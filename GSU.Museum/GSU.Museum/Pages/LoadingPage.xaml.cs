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
                App.Settings = await DependencyService.Get<CachingService>().ReadSettings();
                DependencyService.Get<LocalizationService>().Localize();
            }
            Style style = Application.Current.Resources["TransparentMenuItem"] as Style;
            var width = Math.Round(App.Current.MainPage.Width - (0.1 * App.Current.MainPage.Width));
            style.Setters.Add(new Setter() { Property = HeightRequestProperty, Value = width / 2 });
            style.Setters.Add(new Setter() { Property = WidthRequestProperty, Value = width });
            Application.Current.Resources.Remove("TransparentMenuItem");
            Application.Current.Resources.Add("TransparentMenuItem", style);

            style = Application.Current.Resources["DefaultMenuItem"] as Style;
            style.Setters.Add(new Setter() { Property = HeightRequestProperty, Value = width / 2 });
            style.Setters.Add(new Setter() { Property = WidthRequestProperty, Value = width });
            Application.Current.Resources.Remove("DefaultMenuItem");
            Application.Current.Resources.Add("DefaultMenuItem", style);

            App.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}