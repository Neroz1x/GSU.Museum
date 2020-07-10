using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExhibitsArticle : ContentPage
    {
        public ExhibitsArticle(Exhibit exhibit)
        {
            InitializeComponent();
            BindingContext = new ExhibitsArticleViewModel(exhibit, Navigation);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = BindingContext as ExhibitsArticleViewModel;
            await viewModel.FillPage();
        }
    }
}