using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class HallPageViewModel : BaseViewModel
    {
        #region Fields
        public Command GetStandsCommand { get; }
        public Command SelectStandCommand { get; }

        public ObservableCollection<StandDTO> Stands { get; }

        private string _hallId;

        // Title of the page
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (value != _title)
                {
                    _title = value;
                }
                OnPropertyChanged(nameof(Title));
            }
        }

        #endregion

        public HallPageViewModel(string hallId, INavigation navigation) : base(navigation)
        {
            Navigation = navigation;
            _hallId = hallId;
            Stands = new ObservableCollection<StandDTO>();
            GetStandsCommand = new Command(async () => await GetStandsAsync());
            SelectStandCommand = new Command(async id => await SelectStand(id?.ToString()));
        }

        #region Methods
        public async Task GetStandsAsync()
        {
            await LoadAsync(LoadStandsAsync);
        }

        public async Task LoadStandsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            ContentVisibility = false;
            IsBusy = true;
            var hall = await DependencyService.Get<ContentLoaderService>().LoadHallAsync(_hallId, _cancellationTokenSource.Token);
            Title = $"{hall.Title} - {AppResources.HallPage_Title}";
            Stands.Clear();
            foreach (var stand in hall.Stands)
            {
                if (string.IsNullOrEmpty(stand.Title) || string.IsNullOrEmpty(stand.Description) || stand.Photo?.Photo == null)
                {
                    continue;
                }
                Stands.Add(stand);
            }
            if (Stands.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_InProgress, AppResources.MessageBox_ButtonOk);
                await Navigation.PopAsync();
            }
            else
            {
                SetHeight(Stands.Count);
                ContentVisibility = true;
            }
        }

        public async Task SelectStand(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await Navigation.PushAsync(new StandPage(_hallId, id));
            }
        }

        #endregion
    }
}
