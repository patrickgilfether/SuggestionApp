/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for User.
 * a User has a list of basic suggestions for suggestions they authored and suggestions they voted on
 * 
 */
namespace SuggestionAppLibrary.Models;

public class UserModel
{
    //mark as mongoDb data object
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]


    //declare data members
    public string Id { get; set; }

    public string ObjectIdentifier { get; set; } //azure active directory id

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string DisplayName { get; set; }

    public string EmailAddress { get; set; }

    public List<BasicSuggestionModel> AuthoredSuggestions { get; set; } = new List<BasicSuggestionModel>();

    public List<BasicSuggestionModel> VotedOnSuggestions { get; set; } = new List<BasicSuggestionModel>();



    //public UserModel()
    //{
    //}
}
