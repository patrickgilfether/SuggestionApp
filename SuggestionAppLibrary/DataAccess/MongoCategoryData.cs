/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Class definining CRUD methods for category collection
* 
*/


using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess;

public class MongoCategoryData : ICategoryData
{
    private readonly IMongoCollection<CategoryModel> _categories;
    private readonly IMemoryCache _cache;
    private const string cacheName = "CategoryData"; //corresponds to field name in BSON object. Defined in 

    //constructor
    //takes IDbconnection and IMemoryCache
    public MongoCategoryData(IDbConnection db, IMemoryCache cache)
    {
        _cache = cache;
        _categories = db.CategoryCollection;
    }

    //get all collections
    //initializes and uses a cache to return the categories.
    //Not particularly concurrency safe, but sufficient for our needs
    //returns a list of all connections
    public async Task<List<CategoryModel>> GetAllCategories()
    {
        //get the cache
        var output = _cache.Get<List<CategoryModel>>(cacheName);

        //case where cache is null (e.g. initial run), initialize cache
        if (output is null)
        {
            //get all categories from mongoDB, buffer to output var
            var results = await _categories.FindAsync(_ => true);
            output = results.ToList();
            //cache output buffer as value for cacheName as key
            _cache.Set(cacheName, output, TimeSpan.FromDays(1));
        }

        return output;
    }

    //allows for category creation. Unlikely to be used beyond db initialization.
    //returns result of insertion operation
    public Task CreateCategory(CategoryModel category)
    {
        return _categories.InsertOneAsync(category);
    }


}

