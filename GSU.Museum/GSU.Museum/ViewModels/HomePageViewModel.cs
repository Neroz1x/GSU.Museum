using GSU.Museum.Shared.Pages;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        #region Fields
        public INavigation Navigation;

        public ObservableCollection<ImageSource> Images { get; set; } = new ObservableCollection<ImageSource>();
        public Command NavigateToMainPageCommand { get; }
        public Command NavigateToOptionsPageCommand { get; }

        #endregion
        public HomePageViewModel(INavigation navigation) 
        {
            Navigation = navigation;
            NavigateToMainPageCommand = new Command(async () => await NavigateToMainPage());
            NavigateToOptionsPageCommand = new Command(async () => await NavigateToOptionsPage());
            Images.Add("Emblem.png");
            Images.Add("Flag.jpeg");
            Images.Add("Shield.jpeg");
        }

        #region Methods
        private async Task NavigateToMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async Task NavigateToOptionsPage()
        {
            await Navigation.PushAsync(new OptionsPage());
        }
        #endregion
    }
}