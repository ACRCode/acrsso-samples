using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AcrSso.MvcSample.Controllers
{
    public class AcrSsoController : Controller
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.Message = "This is a sample page that demonstrates how to initiate login process with ACR SSO.";

            var authResult = await HttpContext.GetOwinContext().Authentication.AuthenticateAsync(OpenIdConnectAuthenticationDefaults.AuthenticationType);
            if(authResult != null)
            {
                ViewBag.AccessToken = authResult.Properties.Dictionary["access_token"];
                ViewBag.IdToken = authResult.Properties.Dictionary["id_token"];
                ViewBag.RefreshToken = authResult.Properties.Dictionary["refresh_token"];
            }

            return View();
        }

        public void SignIn()
        {
            HttpContext
                .GetOwinContext()
                .Authentication
                .Challenge(new AuthenticationProperties { RedirectUri = "AcrSso" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }
    }
}