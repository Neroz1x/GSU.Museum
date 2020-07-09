using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum
{
    public partial class App : Application
    {
        public const string UriBase = "10.0.2.2:5001";

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
