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
    public class MainPageViewModel : BaseViewModel
    {
#region Fields 
        public INavigation Navigation;
        public Command GetHallsCommand { get; }
        public Command SelectHallCommand { get; }
        public Command NavigateToHomePageCommand { get; }
        public ObservableCollection<HallDTO> Halls { get; }

        // Status of LoadingIndicator
        private bool _isBusy = false;
        public bool IsBusy 
        { 
            get
            {
                return _isBusy;
            }
           
            set 
            {
                if(value != _isBusy)
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

        // Visibility of reload button
        private bool _reloadButtonVisibility = false;
        public bool ReloadButtonVisibility
        {
            get
            {
                return _reloadButtonVisibility;
            }

            set
            {
                if (value != _reloadButtonVisibility)
                {
                    _reloadButtonVisibility = value;
                }
                OnPropertyChanged(nameof(ReloadButtonVisibility));
            }
        }

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

        #endregion

        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Halls = new ObservableCollection<HallDTO>();
            GetHallsCommand = new Command(async () => await GetHalls());
            SelectHallCommand = new Command(async Id => await SelectHall((string)Id));
            NavigateToHomePageCommand = new Command(() => App.Current.MainPage = new NavigationPage(new HomePage()));
        }

        #region Methods
        public async Task GetHalls()
        {
            Halls.Clear();
            ReloadButtonVisibility = false;
            ContentVisibility = false;
            IsBusy = true;

            try
            {
                var halls = await DependencyService.Get<ContentLoaderService>().LoadHallsAsync();
                Halls.Clear();
                foreach (var hall in halls)
                {
                    if(string.IsNullOrEmpty(hall.Title))
                    {
                        continue;
                    }
                    Halls.Add(hall);
                }
                if(halls.Count == 0)
                {
                    ReloadButtonVisibility = true;
                }
                else
                {
                    var height = Math.Round(App.Current.MainPage.Width - (0.1 * App.Current.MainPage.Width)) / 2;
                    height = height * Halls.Count + (Halls.Count - 1) * 20 + 1;
                    CollectionViewHeight = height;
                    ContentVisibility = true;
                }
            }
            catch(Exception ex)
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
                }
                else if (ex is HttpRequestException || ex is WebException)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, ex.Message, AppResources.MessageBox_ButtonOk);
                }
                ContentVisibility = false;
                ReloadButtonVisibility = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task SelectHall(string id)
        {
            await Navigation.PushAsync(new HallPage(id));
        }
        
        public async Task NavigateToOptionsPage()
        {
            await Navigation.PushAsync(new OptionsPage());
        }
        #endregion
    }
}
