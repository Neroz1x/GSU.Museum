
using GSU.Museum.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HallPage : ContentPage
    {
        public HallPage(string id)
        {
            InitializeComponent();
            BindingContext = new HallPageViewModel(id, Navigation);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            var viewModel = BindingContext as HallPageViewModel;
            await viewModel.GetStands();
        }
    }
}