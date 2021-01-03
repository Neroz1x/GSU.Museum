using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.ViewModels;
using PanCardView.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExhibitGallery : ContentPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibit">Exhibit to display</param>
        public ExhibitGallery(ExhibitDTO exhibit)
        {
            InitializeComponent();
            BindingContext = new ExhibitGalleryViewModel(exhibit, Navigation);
        }
        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = BindingContext as ExhibitGalleryViewModel;
            viewModel.FillPage();
            NavigationLabel.Text = $"{Carousel.Position + 1}/{Carousel.ItemsSource.Count()}";
        }

        // Set navigation panel label text
        private void CarouselView_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            NavigationLabel.Text = $"{Carousel.Position + 1} / {Carousel.ItemsSource.Count()}";
        }

        // Set next item in carousel view
        private void ShowNext(object sender, System.EventArgs e)
        {
            if (Carousel.Position != Carousel.ItemsSource.Count() - 1)
            {
                Carousel.ScrollTo(Carousel.Position + 1);
            }
        }

        // Set previous item in carousel view
        private void ShowPrevious(object sender, System.EventArgs e)
        {
            if (Carousel.Position != 0)
            {
                Carousel.ScrollTo(Carousel.Position - 1);
            }
        }
    }
}