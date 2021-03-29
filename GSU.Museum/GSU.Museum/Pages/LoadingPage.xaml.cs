using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            try
            {
                App.Settings = await DependencyService.Get<CachingService>().ReadSettings();
                DependencyService.Get<LocalizationService>().Localize();
            }
            catch (Exception) { App.Settings = new Settings(); }

            // Edit menu item style depending on screen width
            Style style = Application.Current.Resources["TransparentMenuItem"] as Style;
            var screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            var width = Math.Round(screenWidth - (0.1 * screenWidth));
            style.Setters.Add(new Setter() { Property = HeightRequestProperty, Value = width / 2 });
            style.Setters.Add(new Setter() { Property = WidthRequestProperty, Value = width });
            Application.Current.Resources.Remove("TransparentMenuItem");
            Application.Current.Resources.Add("TransparentMenuItem", style);
            
            style = Application.Current.Resources["CarouselViewImage"] as Style;
            width = Math.Round(screenWidth - (0.35 * screenWidth));
            style.Setters.Add(new Setter() { Property = HeightRequestProperty, Value = width });
            Application.Current.Resources.Remove("CarouselViewImage");
            Application.Current.Resources.Add("CarouselViewImage", style);

            App.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}