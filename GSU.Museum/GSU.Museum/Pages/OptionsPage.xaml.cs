using GSU.Museum.Shared.Services;
using GSU.Museum.Shared.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GSU.Museum.Shared.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OptionsPage : ContentPage
    {
        public OptionsPage()
        {
            InitializeComponent();
            BindingContext = new OptionsPageViewModel(Navigation);
        }

        private async void ContentPage_Disappearing(object sender, System.EventArgs e)
        {
            await OnPageExit();
        }

        async Task OnPageExit()
        {
            await DependencyService.Get<CachingService>().WriteSettings();
            Application.Current.MainPage = new NavigationPage(new HomePage());
        }
    }
}