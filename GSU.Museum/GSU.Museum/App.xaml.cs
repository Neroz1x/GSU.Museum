using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Pages;
using Xamarin.Forms;

namespace GSU.Museum
{
    public partial class App : Application
    {
        public const string UriBase = "https://gsumuseumapi.azurewebsites.net";
        
        // For testing locally
        //public const string UriBase = "https://10.0.2.2:5001";

        public static Settings Settings { get; set; }

        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        
        public App()
        {
            InitializeComponent();

            Akavache.Registrations.Start("GSU.Museum");
            MainPage = new NavigationPage(new LoadingPage());
        }

        protected override void OnStart()
        {
            _logger.Info("Application is running");
        }

        protected override void OnSleep()
        {
            _logger.Info("Application is shut down");
        }

        protected override void OnResume()
        {
        }

    }
}
