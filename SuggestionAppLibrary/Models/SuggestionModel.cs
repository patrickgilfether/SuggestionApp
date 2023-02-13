/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for Suggestion
 * 
 */
namespace SuggestionAppLibrary.Models;

public class SuggestionModel
{
    //mark as mongoDb data object
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]


    //declare data members
    public string Id { get; set; }
    public string Suggestion { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public CategoryModel Category { get; set; }
    public BasicUserModel Author { get; set; }
    public HashSet<string> UserVotes { get; set; } = new HashSet<string>();
    public StatusModel SuggestionStatus { get; set; }
    public string OwnerNotes { get; set; }
    public bool ApprovedForRelease { get; set; } = false;
    public bool Archived { get; set; } = false;
    public bool Rejected { get; set; } = false;
}

