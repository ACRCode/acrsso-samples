using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AcrSso.WpfSample
{
    public partial class MainWindow : Window, IBrowser
    {
        private MainViewModel _mainViewModel;
        private BrowserResult _result;
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0, 1);
        private BrowserOptions _options;

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            DataContext = _mainViewModel;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            await _mainViewModel.Login(this);
        }

        private void SsoWebBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.EndUrl) && e.Uri.OriginalString.StartsWith(_options.EndUrl) && _options.ResponseMode == OidcClientOptions.AuthorizeResponseMode.Redirect)
            {
                _result.ResultType = BrowserResultType.Success;
                _result.Response = e.Uri.OriginalString;
                _signal.Release();
            }
        }

        private void SsoWebBrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (!string.IsNullOrEmpty(_options.EndUrl) && e.Uri.OriginalString.StartsWith(_options.EndUrl))
            {
                e.Cancel = true;
                _result.ResultType = BrowserResultType.Success;
                if (_options.ResponseMode == OidcClientOptions.AuthorizeResponseMode.Redirect)
                {
                    _result.Response = e.Uri.OriginalString;
                }
                _signal.Release();
            }
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            _options = options;
            _result = new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel
            };

            SsoWebBrowser.Navigate(options.StartUrl);
            await _signal.WaitAsync();

            return _result;
        }
    }
}
