/**Patrick Gilfether
* 02/08/2023
* Suggestions Web App
* Singleton Class establishing connection to MongoDb database.
* Could be extended to a scoped class if multiple connections needed to be established
* 
*/
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SuggestionAppLibrary.DataAccess;

public class DbConnection
{
    //interfaces
    private readonly IConfiguration _config; //appsettings.json
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";//hardcoded name used to locate DB in appsettings.json

    public string DbName { get; private set; }

    //collection name strings
    public string CategoryCollectionName { get; private set; } = "categories";
    public string StatusCollectionName { get; private set; } = "statuses";
    public string UserCollectionName { get; private set; } = "users";
    public string SuggestionCollectionName { get; private set; } = "suggestions";
    //client and collections
    public MongoClient Client { get; private set; }
    public IMongoCollection<CategoryModel> CategoryCollection{ get; private set;}
    public IMongoCollection<StatusModel> StatusCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<SuggestionModel> SuggestionCollection { get; private set; }

    //constructor
    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);

        //connect collection interfaces to db.collections using CollectionNames
        CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
        StatusCollection = _db.GetCollection<StatusModel>(StatusCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        SuggestionCollection = _db.GetCollection<SuggestionModel>(SuggestionCollectionName);

    }
}
