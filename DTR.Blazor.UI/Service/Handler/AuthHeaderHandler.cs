using System.Net.Http.Headers;

namespace DTR.Blazor.UI.Service.Handler
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public AuthHeaderHandler(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        //DelegatingHandler is used to AUTOMATICALLY attached generated token of TOKEN SERVICE to every Http Request
        //This setup is standard and optiize for me
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenService.GetTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
