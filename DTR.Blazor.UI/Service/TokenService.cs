using DTR.Blazor.UI.Model;
using System.IdentityModel.Tokens.Jwt;

namespace DTR.Blazor.UI.Service
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private string _token;
        private DateTime _tokenExpiry;

        private readonly string _clientId = "machine_id";
        private readonly string _clientSecret = "very_secret";

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_token) || DateTime.UtcNow >= _tokenExpiry.AddMinutes(-5))
            {
                await RefreshTokenAsync();
            }

            return _token;
        }

        private async Task RefreshTokenAsync()
        {
            var authRequest = new
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7222/api/Auth/Token", authRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

                _token = result.Token;

                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_token);
                _tokenExpiry = jwt.ValidTo;
            }
            else
            {
                throw new Exception("Failed to refresh token");
            }
        }
    }
}
