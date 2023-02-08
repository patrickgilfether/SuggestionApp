/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for Category
 * 
 */
namespace SuggestionAppLibrary.Models;

public class CategoryModel
{
	//mark as mongoDb data object
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]


    //declare data members
    public string Id { get; set; }
	public string CategoryName { get; set; }
	public string CategoryDescription { get; set; }


	////constructor
	//public CategoryModel()
	//{
	//}
}

