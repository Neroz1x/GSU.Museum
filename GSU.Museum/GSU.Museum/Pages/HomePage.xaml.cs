using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Services;
using GSU.Museum.Shared.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private bool _isActiveCarousel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomePageViewModel(Navigation);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            // Read settings
            if (App.Settings == null)
            {
                try
                {
                    App.Settings = await DependencyService.Get<CachingService>().ReadSettings();
                    DependencyService.Get<LocalizationService>().Localize();
                }
                catch (Exception) { App.Settings = new Settings(); }

                // Edit menu item style depending on screen width
                Style style = Application.Current.Resources["TransparentMenuItem"] as Style;
                var width = Math.Round(App.Current.MainPage.Width - (0.1 * App.Current.MainPage.Width));
                style.Setters.Add(new Setter() { Property = HeightRequestProperty, Value = width / 2 });
                style.Setters.Add(new Setter() { Property = WidthRequestProperty, Value = width });
                Application.Current.Resources.Remove("TransparentMenuItem");
                Application.Current.Resources.Add("TransparentMenuItem", style);
            }

            // Start carousel
            _isActiveCarousel = true;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (Carousel.SelectedIndex == Carousel.ItemsCount - 1)
                {
                    Carousel.SelectedIndex = 0;
                }
                else
                {
                    Carousel.SelectedIndex += 1;
                }
                return _isActiveCarousel;
            });
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            _isActiveCarousel = false;
        }
    }
}