using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Web;
using System.Web.UI;

namespace AcrSso.WebFormsSample
{
    public partial class AcrSso : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var authWaiter = HttpContext.Current.GetOwinContext().Authentication.AuthenticateAsync(OpenIdConnectAuthenticationDefaults.AuthenticationType);
            authWaiter.Wait();
            var authResult = authWaiter.Result;
            if (authResult != null)
            {
                AccessToken.Text  = authResult.Properties.Dictionary["access_token"];
                IdToken.Text      = authResult.Properties.Dictionary["id_token"];
                RefreshToken.Text = authResult.Properties.Dictionary["refresh_token"];
                TokensPanel.Visible = true;
            }
            else
            {
                TokensPanel.Visible = false;
            }
        }

        protected void SignIn_Click(object sender, EventArgs e)
        {
            HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "AcrSso" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }
    }
}