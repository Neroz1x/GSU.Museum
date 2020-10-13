using GSU.Museum.Shared.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandPage : ContentPage
    {
        public StandPage(string hallId, string id)
        {
            InitializeComponent();
            BindingContext = new StandPageViewModel(hallId, id, Navigation);
        }

        private async void ContentPage_Appearing(object sender, System.EventArgs e)
        {
            if (ExhibitsCollectionView.SelectedItem == null)
            {
                var viewModel = BindingContext as StandPageViewModel;
                await viewModel.GetExhibitsAsync();
            }
            else
            {
                ExhibitsCollectionView.SelectedItem = null;
            }
        }

        private void ContentPage_Disappearing(object sender, System.EventArgs e)
        {
            var viewModel = BindingContext as StandPageViewModel;
            viewModel.Cancel();
        }
    }
}