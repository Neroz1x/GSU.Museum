using GSU.Museum.Shared.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (App.Settings == null)
            {
                App.Settings = await Xamarin.Forms.DependencyService.Get<CachingService>().ReadSettings();
                Xamarin.Forms.DependencyService.Get<LocalizationService>().Localize();
            }
            App.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}