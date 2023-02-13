/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Class definining methods for user collection
* 
*/
namespace SuggestionAppLibrary.DataAccess;

public class MongoUserData : IUserData
{
    private readonly IMongoCollection<UserModel> _users;

    public MongoUserData(IDbConnection db)
    {
        _users = db.UserCollection;
    }

    //gets all users, returns as list
    public async Task<List<UserModel>> GetUsersAsync()
    {
        var results = await _users.FindAsync(_ => true);
        return results.ToList();
    }

    //gets a user by an id string, returns first match
    public async Task<UserModel> GetUser(string id)
    {
        var results = await _users.FindAsync(u => u.Id == id);
        return results.FirstOrDefault();
    }

    //gets a user by Azure Active Directory id, returns first match.
    public async Task<UserModel> GetUserFromAuthentication(string objectId)
    {
        var results = await _users.FindAsync(u => u.ObjectIdentifier == objectId);
        return results.FirstOrDefault();
    }

    //Takes a user model. Inserts the user model as a new user (allows duplicates). returns the result of the operation
    public Task CreateUser(UserModel user)
    {
        return _users.InsertOneAsync(user);
    }

    //Takes a user model. Replaces the first object matching that Id with user object passed in.
    //Upserts (meaning it will control for duplicates).
    //Returns result.
    public Task UpdateUser(UserModel user)
    {
        var filter = Builders<UserModel>.Filter.Eq("Id", user.Id);
        return _users.ReplaceOneAsync(filter, user, new ReplaceOptions { IsUpsert = true });
    }

}

