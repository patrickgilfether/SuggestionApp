/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for Status (a suggestion has a status)
 * 
 */

namespace SuggestionAppLibrary.Models;

public class StatusModel
{
    //mark as mongoDb data object
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]


    //declare data members
    public string Id { get; set; }
    public string StatusName { get; set; }
    public string StatusDescription { get; set; }

    //public StatusModel()
    //{
    //}
}

