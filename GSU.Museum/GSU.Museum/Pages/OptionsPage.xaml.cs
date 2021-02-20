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
            BindingContext = new OptionsPageViewModel(Navigation, RadioGroup, CacheGroup);
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

        private void RuFlag_Tapped(object sender, System.EventArgs e)
        {
            RuRadioButton.IsChecked = true;
        }

        private void EnFlag_Tapped(object sender, System.EventArgs e)
        {
            EnRadioButton.IsChecked = true;
        }

        private void BeFlag_Tapped(object sender, System.EventArgs e)
        {
            BeRadioButton.IsChecked = true;
        }

        private void RuCache_Tapped(object sender, System.EventArgs e)
        {
            RuCheckBox.IsChecked = !RuCheckBox.IsChecked;
        }

        private void EnCache_Tapped(object sender, System.EventArgs e)
        {
            EnCheckBox.IsChecked = !EnCheckBox.IsChecked;
        }

        private void BeCache_Tapped(object sender, System.EventArgs e)
        {
            BeCheckBox.IsChecked = !BeCheckBox.IsChecked;
        }
    }
}