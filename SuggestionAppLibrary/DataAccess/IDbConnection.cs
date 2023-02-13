/**Patrick Gilfether
* 02/08/2023
* Suggestions Web App
* Interface defining Singleton connection to MongoDb database.
* 
*/
using MongoDB.Driver;

namespace SuggestionAppLibrary.DataAccess
{
    public interface IDbConnection
    {
        string DbName { get; }
        string CategoryCollectionName { get; }
        string StatusCollectionName { get; }
        string UserCollectionName { get; }
        string SuggestionCollectionName { get; }
        MongoClient Client { get; }
        IMongoCollection<CategoryModel> CategoryCollection { get; }
        IMongoCollection<StatusModel> StatusCollection { get; }
        IMongoCollection<UserModel> UserCollection { get; }
        IMongoCollection<SuggestionModel> SuggestionCollection { get; }
    }
}