using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExhibitsArticle : ContentPage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibit">Exhibit to display</param>
        public ExhibitsArticle(ExhibitDTO exhibit)
        {
            InitializeComponent();
            BindingContext = new ExhibitsArticleViewModel(exhibit, Navigation);
        }

        private void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = BindingContext as ExhibitsArticleViewModel;
            viewModel.FillPage();
        }
    }
}