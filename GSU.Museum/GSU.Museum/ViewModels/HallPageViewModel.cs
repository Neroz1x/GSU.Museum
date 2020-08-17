using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class HallPageViewModel : BaseViewModel
    {
        #region Fields
        public INavigation Navigation;
        public Command GetStandsCommand { get; }
        public Command SelectStandCommand { get; }
        public Command NavigateToHomePageCommand { get; }
        public ObservableCollection<StandDTO> Stands { get; }

        // Visibility of page content
        private bool _contentVisibility = true;
        public bool ContentVisibility
        {
            get
            {
                return _contentVisibility;
            }

            set
            {
                if (value != _contentVisibility)
                {
                    _contentVisibility = value;
                }
                OnPropertyChanged(nameof(ContentVisibility));
            }
        }

        // Status of LoadingIndicator
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                }
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        // Height of collection vire
        private double _collectionViewHeight;
        public double CollectionViewHeight
        {
            get
            {
                return _collectionViewHeight;
            }

            set
            {
                if (value != _collectionViewHeight)
                {
                    _collectionViewHeight = value;
                }
                OnPropertyChanged(nameof(CollectionViewHeight));
            }
        }

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

        private string _hallId;
        #endregion

        public HallPageViewModel(string hallId, INavigation navigation)
        {
            Navigation = navigation;
            _hallId = hallId;
            Stands = new ObservableCollection<StandDTO>();
            GetStandsCommand = new Command(async () => await GetStands());
            SelectStandCommand = new Command(async id => await SelectStand(id.ToString()));
            NavigateToHomePageCommand = new Command(() => App.Current.MainPage = new NavigationPage(new HomePage()));
        }

        #region Methods
        public async Task GetStands()
        {
            try
            {
                ContentVisibility = false;
                IsBusy = true;
                var hall = await DependencyService.Get<ContentLoaderService>().LoadHallAsync(_hallId);
                Title = $"{hall.Title} - {AppResources.HallPage_Title}";
                Stands.Clear();
                foreach (var stand in hall.Stands)
                {
                    if(string.IsNullOrEmpty(stand.Title) || string.IsNullOrEmpty(stand.Description) || stand.Photo?.Photo == null)
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
                    var height = Math.Round(App.Current.MainPage.Width - (0.1 * App.Current.MainPage.Width)) / 2;
                    height = height * Stands.Count + (Stands.Count - 1) * 20 + 1;
                    CollectionViewHeight = height;
                    ContentVisibility = true;
                }
            }
            catch (Exception ex)
            {
                if (ex is Error error)
                {
                    if (error.ErrorCode == CommonClassLibrary.Enums.Errors.Failed_Connection)
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, error.Info, AppResources.MessageBox_ButtonOk);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, error.Info, AppResources.MessageBox_ButtonOk);
                    }
                    await Navigation.PopAsync();
                }
                else if(ex is HttpRequestException || ex is WebException)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
                    await Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, ex.Message, AppResources.MessageBox_ButtonOk);
                    await Navigation.PopAsync();
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SelectStand(string id)
        {
            await Navigation.PushAsync(new StandPage(_hallId, id));
        }
        #endregion
    }
}
