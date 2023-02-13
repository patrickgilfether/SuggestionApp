/**Patrick Gilfether
* 02/13/2023
* Suggestions Web App
* Class definining data access methods for status collection
* 
*/

namespace SuggestionAppLibrary.DataAccess
{
    public interface IStatusData
    {
        Task CreateStatuses(StatusModel status);
        Task<List<StatusModel>> GetAllStatuses();
    }
}