using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum
{
    public partial class App : Application
    {
        public const string UriBase = "10.0.2.2:5001";
        private readonly NLog.ILogger _logger = NLog.LogManager.GetCurrentClassLogger();
        public App()
        {
            InitializeComponent();

            Akavache.Registrations.Start("GSU.Museum");
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            _logger.Info("Application is running");
        }

        protected override void OnSleep()
        {
            _logger.Info("Application is shut down");
            NLog.LogManager.Shutdown();
        }

        protected override void OnResume()
        {
        }
    }
}
