using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace AcrSso.WpfSample
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _authority = "https://sso.acr.org/oauth2/default";
        private string _additionalEndpoints = "https://sso.acr.org/oauth2/v1/";
        private string _clientId = "[Client ID]";
        private string _clientSecret = "[Client Secret]";
        private string _redirectUrl = "[Redirect URL]";
        private string _scopes = "openid offline_access lcsr_data_submission";
        private string _accessToken;
        private string _idToken;
        private string _refreshToken;
        private OidcClient _oidcClient;

        public MainViewModel()
        {
            var options = new OidcClientOptions
            {
                Authority = Authority,
                ClientId = ClientId,
                ClientSecret = ClientSecret,
                Scope = Scopes,
                RedirectUri = RedirectUrl,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
            };

            options.Policy.Discovery.AdditionalEndpointBaseAddresses.Add(AdditionalEndpoints);

            _oidcClient = new OidcClient(options);
        }

        public string Authority
        {
            get => _authority;
            set => SetProperty(ref _authority, value);
        }

        public string AdditionalEndpoints
        {
            get => _additionalEndpoints;
            set => SetProperty(ref _additionalEndpoints, value);
        }
        
        public string ClientId
        {
            get => _clientId;
            set => SetProperty(ref _clientId, value);
        }

        public string ClientSecret
        {
            get => _clientSecret;
            set => SetProperty(ref _clientSecret, value);
        }

        public string RedirectUrl
        {
            get => _redirectUrl;
            set => SetProperty(ref _redirectUrl, value);
        }

        public string Scopes
        {
            get => _scopes;
            set => SetProperty(ref _scopes, value);
        }
        public string AccessToken
        {
            get => _accessToken;
            set => SetProperty(ref _accessToken, value);
        }

        public string IdToken
        {
            get => _idToken;
            set => SetProperty(ref _idToken, value);
        }

        public string RefreshToken
        {
            get => _refreshToken;
            set => SetProperty(ref _refreshToken, value);
        }

        public object MessageBoxButtons { get; private set; }

        public async Task Login(IBrowser browser)
        {
            _oidcClient.Options.Authority    = Authority;
            _oidcClient.Options.ClientId     = ClientId;
            _oidcClient.Options.ClientSecret = ClientSecret;
            _oidcClient.Options.Scope        = Scopes;
            _oidcClient.Options.RedirectUri  = RedirectUrl;
            _oidcClient.Options.Browser      = browser;

            var result = await _oidcClient.LoginAsync(new LoginRequest { BrowserDisplayMode = DisplayMode.Visible});

            if (result.IsError)
            {
                MessageBox.Show(result.Error, "Login", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AccessToken  = result.AccessToken;
            IdToken      = result.IdentityToken;
            RefreshToken = result.RefreshToken;
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T prop, T newValue, [CallerMemberName] string propertyName = null)
        {
            if ((prop == null && newValue == null) || (prop != null && prop.Equals(newValue))) return;

            prop = newValue;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation
    }
}
