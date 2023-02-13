/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Interface definining CRUD methods for category collection
* 
*/


namespace SuggestionAppLibrary.DataAccess
{
    public interface ICategoryData
    {
        Task CreateCategory(CategoryModel category);
        Task<List<CategoryModel>> GetAllCategories();
    }
}