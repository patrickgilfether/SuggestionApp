/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for basic suggestion
 * overload constructor maps user to basic user
 * this allows less data to be stored in interacting objects/documents
 * 
 */
namespace SuggestionAppLibrary.Models;

public class BasicUserModel
{
	[BsonRepresentation(BsonType.ObjectId)]

	public string Id { get; set; }

	public string DisplayName { get; set; }


	public BasicUserModel()
	{
	}

	public BasicUserModel(UserModel user)
	{
		Id = user.Id;
		DisplayName = user.DisplayName;
	}
}

