
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Dima.Web.Security
{
    public class CookieHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            request.Headers.Add("x-Requested-With", ["XMLHttpRequest"]);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
