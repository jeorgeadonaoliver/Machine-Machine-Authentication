using DTR.Blazor.UI.Service;

using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;

namespace DTR.Blazor.UI.Components.Pages;

public partial class ProtectedData
{
    [Inject]
    private TokenService tokenService { get; set; }

    private string message;
    private string token;

    protected override async Task OnInitializedAsync()
    {      
        try
        {
            //Get Api endpoint that generates token
            token = await tokenService.GetTokenAsync();

            //you can find this in your program.cs
            // you need this bloc of code to access AuthHeaderHandler
            // this is GET Request
            var client = ClientFactory.CreateClient("AuthorizedApiClient");
            var response = await client.GetAsync("api/Secret");

            message = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            message = $"Error: {ex.Message}";
        }
    }
}