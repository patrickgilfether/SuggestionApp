/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Class definining data access methods for status collection
* 
*/

using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess;

public class MongoStatusData : IStatusData
{
    private readonly IMemoryCache _cache;
    private readonly IMongoCollection<StatusModel> _statuses;
    private const string cacheName = "StatusData";


    public MongoStatusData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _statuses = db.StatusCollection;

    }

    //getter for statuses. Uses and initializes a cache to minimize db io.
    public async Task<List<StatusModel>> GetAllStatuses()
    {
        var output = _cache.Get<List<StatusModel>>(cacheName);

        //init cache if null
        if (output is null)
        {
            var results = await _statuses.FindAsync(_ => true);
            output = results.ToList();
            _cache.Set(cacheName, results, TimeSpan.FromDays(1));
        }
        return output;
    }

    //insert statuses, used primarily for initialization and testing
    public Task CreateStatuses(StatusModel status)
    {
        return _statuses.InsertOneAsync(status);
    }
}

