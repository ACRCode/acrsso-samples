using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;

namespace AcrSso.MvcSample
{
    public class Startup
    {
        private static readonly string Authority = ConfigurationManager.AppSettings["Authority"];
        private static readonly string ClientId = ConfigurationManager.AppSettings["ClientId"];
        private static readonly string ClientSecret = ConfigurationManager.AppSettings["ClientSecret"];
        private static readonly string Scopes = ConfigurationManager.AppSettings["Scopes"];
        private static readonly string RedirectUrl = ConfigurationManager.AppSettings["RedirectUrl"];

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(OpenIdConnectAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions() { AuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType });

            var oidcOptions = new OpenIdConnectAuthenticationOptions(OpenIdConnectAuthenticationDefaults.AuthenticationType)
            {
                SignInAsAuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType,
                Authority = Authority,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                ResponseType = "code",
                Scope = Scopes,
                RedirectUri = RedirectUrl,
                RedeemCode = true,
                SaveTokens = true, // this is only needed for demonstration purpose to make sure the tokens are stored in the cookies and as awailable on the AcrSso page to display.
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    TokenResponseReceived = OnTokenResponseReceived
                }
            };
            app.UseOpenIdConnectAuthentication(oidcOptions);

            // This makes any middleware defined above this line run before the Authorization rule is applied in web.config
            app.UseStageMarker(PipelineStage.Authenticate);
        }

        private async Task OnTokenResponseReceived(TokenResponseReceivedNotification arg)
        {
            //Put you code that stores to tokens to future use
            var accessToken = arg.TokenEndpointResponse.AccessToken;
            var refreshToken = arg.TokenEndpointResponse.RefreshToken;
        }
    }
}