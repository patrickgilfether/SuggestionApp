/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Class definining methods for suggestions data
* 
*/
using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess;

public class MongoSuggestionData : ISuggestionData
{
    private IMongoCollection<SuggestionModel> _suggestions;
    private readonly IDbConnection _db;
    private IUserData _userData;
    private IMemoryCache _cache;
    private const string CacheName = "SuggestionData";

    public MongoSuggestionData(IDbConnection db, IUserData userData, IMemoryCache cache)
    {
        _suggestions = db.SuggestionCollection;
        _db = db;
        _userData = userData;
        _cache = cache;
    }

    public async Task<List<SuggestionModel>> GetAllSuggestions()
    {
        var output = _cache.Get<List<SuggestionModel>>(CacheName);

        //initialize cache if null
        if (output is null)
        {
            //fill cache with non archived suggestions
            var results = await _suggestions.FindAsync(s => s.Archived == false);
            output = results.ToList();
            //expire the cache every minute
            _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
        }
        return output;
    }

    //getter for approved posts
    public async Task<List<SuggestionModel>> GetAllApprovedSuggestions()
    {
        //reuse get all and filter out the non-approved posts
        var output = await GetAllSuggestions();
        return output.Where(x => x.ApprovedForRelease).ToList();
    }

    //get one suggestion by id from db
    public async Task<SuggestionModel> GetSuggestion(string id)
    {
        var results = await _suggestions.FindAsync(s => s.Id == id);
        return results.FirstOrDefault();
    }
    //filters the cache to return posts which have neither been rejected nor approved
    public async Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApproval()
    {

        var output = await GetAllSuggestions();
        return output.Where(s =>
            s.ApprovedForRelease == false
            && s.Rejected == false).ToList();
    }

    public async Task UpdateSuggestion(SuggestionModel suggestion)
    {
        await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id, suggestion);
        //destroy cache, as it is now stale
        _cache.Remove(CacheName);

    }


    public async Task UpvoteSuggestion(string suggestionId, string userId)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        //use transaction to ensure completion of upvote procedure
        session.StartTransaction();

        try
        {
            //create new client for sake of transaction... honestly, not sure why this needs to happen...
            var db = client.GetDatabase(_db.DbName);
            var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionCollectionName);
            var suggestion = (await suggestionsInTransaction.FindAsync(s => s.Id == suggestionId)).First(); //throws error if suggestion not found

            //try to add upvote to hashset. If already upvoted, we remove the upvote.
            //This limits upvotes to one, per user, per post
            bool isUpvote = suggestion.UserVotes.Add(userId);
            if (isUpvote == false)
            {
                suggestion.UserVotes.Remove(userId);
            }
            //replace the target suggestion with new version containing update uservote hashset
            await suggestionsInTransaction.ReplaceOneAsync(s => s.Id == suggestionId, suggestion);

            //update the user voted on fields
            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(userId);

            if (isUpvote)
            {
                user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
            }
            else
            {
                var suggestionToRemove = user.VotedOnSuggestions.Where(s => s.Id == suggestionId).First();
                user.VotedOnSuggestions.Remove(suggestionToRemove);
            }
            await usersInTransaction.ReplaceOneAsync(u => u.Id == userId, user);

            //complete the transaction
            await session.CommitTransactionAsync();
            //destroy cache to prevent multiple upvotes
            _cache.Remove(CacheName);


        }
        catch (Exception ex)
        {
            //Logging would go here.

            //If upvote transaction fails, we'll escalate that to the caller
            await session.AbortTransactionAsync();
            throw;
        }
    }

    public async Task CreateSuggestion(SuggestionModel suggestion)
    {
        var client = _db.Client;
        using var session = await client.StartSessionAsync();
        session.StartTransaction();

        try
        {
            var db = client.GetDatabase(_db.DbName);
            var suggestionsInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionCollectionName);
            //insert new suggestion
            await suggestionsInTransaction.InsertOneAsync(suggestion);

            //find and update corresponding user record
            var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
            var user = await _userData.GetUser(suggestion.Author.Id);
            user.AuthoredSuggestions.Add(new BasicSuggestionModel(suggestion));
            await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);
            await session.CommitTransactionAsync();
            //we don't invalidate cache here, as the suggestion will be pending admin review anyhow.
        }
        catch (Exception ex)
        {
            //logging could go here
            await session.AbortTransactionAsync();
            throw;
        }
    }
}

