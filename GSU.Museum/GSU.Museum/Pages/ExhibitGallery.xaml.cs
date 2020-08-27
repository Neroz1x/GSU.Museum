using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.ViewModels;
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
        }
    }
}