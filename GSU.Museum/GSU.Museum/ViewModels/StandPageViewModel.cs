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
    public class StandPageViewModel : BaseViewModel
    {
        #region Fields
        public Command GetExhibitsCommand { get; }
        public Command SelectExhibitCommand { get; }
        public ObservableCollection<ExhibitDTO> Exhibits { get; }

        private string _hallId;
        private string _standId;

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

        public StandPageViewModel(string hallId, string standId, INavigation navigation) : base(navigation)
        {
            _hallId = hallId;
            _standId = standId;
            Exhibits = new ObservableCollection<ExhibitDTO>();
            GetExhibitsCommand = new Command(async () => await GetExhibitsAsync());
            SelectExhibitCommand = new Command(async id => await SelectExhibitAsync(id?.ToString()));
        }

        #region Methods
        public async Task GetExhibitsAsync()
        {
            await LoadAsync(LoadExhibitsAsync);
        }

        public async Task LoadExhibitsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var stand = await DependencyService.Get<ContentLoaderService>().LoadStandAsync(_hallId, _standId, _cancellationTokenSource.Token);
            Title = $"{stand.Title} - {AppResources.StandPage_Title}";
            Exhibits.Clear();
            foreach (var exhibit in stand.Exhibits)
            {
                if (string.IsNullOrEmpty(exhibit.Title))
                {
                    continue;
                }
                Exhibits.Add(exhibit);
            }
            if (Exhibits.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_InProgress, AppResources.MessageBox_ButtonOk);
                await Navigation.PopAsync();
            }
            else
            {
                SetHeight(Exhibits.Count);
                ContentVisibility = true;
            }
        }

        public async Task SelectExhibitAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                await LoadAsync(() => LoadExhibitAsync(id));
            }
        }

        public async Task LoadExhibitAsync(string id)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var exhibit = await DependencyService.Get<ContentLoaderService>().LoadExhibitAsync(_hallId, _standId, id, _cancellationTokenSource.Token);
            if (exhibit.ExhibitType == CommonClassLibrary.Data.Enums.ExhibitType.Article)
            {
                await Navigation.PushAsync(new ExhibitsArticle(exhibit));
            }
            else
            {
                await Navigation.PushAsync(new ExhibitGallery(exhibit));
            }
            ContentVisibility = true;
        }
        #endregion
    }
}
