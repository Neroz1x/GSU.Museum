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

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            _isActiveCarousel = true;
            Device.StartTimer(TimeSpan.FromSeconds(5), () =>
            {
                if (Carousel.Position == 2)
                {
                    // Carousel.IsScrollAnimated = false;
                    Carousel.Position = 0;
                    // Carousel.IsScrollAnimated = true;
                }
                Carousel.Position += 1;
                return _isActiveCarousel;
            });
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            _isActiveCarousel = false;
        }
    }
}