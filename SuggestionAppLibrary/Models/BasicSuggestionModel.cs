/**Patrick Gilfether
 * 02/08/2023
 * Suggestions Web App
 * class defining data model for basic suggestion
 * overload constructor maps suggestion to basic suggestion
 * this allows less data to be stored in User objects/documents
 * 
 */

namespace SuggestionAppLibrary.Models;

public class BasicSuggestionModel
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Suggestion { get; set; }

    //no arg constructor. defined explicitly because of overload below
    public BasicSuggestionModel()
    {

    }

    //constructor SuggestionModel overload. Maps existing suggestion model to simplified one.
    public BasicSuggestionModel(SuggestionModel suggestion)
    {
        Id = suggestion.Id;
        Suggestion = suggestion.Suggestion;
    }



}

