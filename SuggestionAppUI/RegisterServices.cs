/**Patrick Gilfether
 * 02/07/2023
 * Suggestions Web App
 * this class is a delegate to manage dependency injection for the program.cs main
 * 
 */
using System;
namespace SuggestionAppUI;

public static class RegisterServices
{
    //does this implicitly establish inheiritance?
	public static void ConfigureServices(this WebApplicationBuilder builder)
	{
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMemoryCache();
    }
}

