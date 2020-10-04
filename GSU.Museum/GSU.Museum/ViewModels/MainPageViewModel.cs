using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
#region Fields 
        public INavigation Navigation;

        private CancellationTokenSource _cancellationTokenSource;

        public Command GetHallsCommand { get; }
        public Command SelectHallCommand { get; }
        public Command NavigateToHomePageCommand { get; }
        public Command NavigateBackCommand { get; }
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
        // Height of collection view
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

        #endregion

        public MainPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Halls = new ObservableCollection<HallDTO>();
            GetHallsCommand = new Command(async () => await GetHalls());
            SelectHallCommand = new Command(async id => await SelectHall(id?.ToString()));
            NavigateToHomePageCommand = new Command(() => App.Current.MainPage = new NavigationPage(new HomePage()));
            NavigateBackCommand = new Command(async () => await navigation.PopAsync());
        }

        #region Methods
        public async Task GetHalls()
        {
            Halls.Clear();
            ContentVisibility = false;
            IsBusy = true;

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                var halls = await DependencyService.Get<ContentLoaderService>().LoadHallsAsync(_cancellationTokenSource.Token);
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
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_InProgress, AppResources.MessageBox_ButtonOk);
                    await Navigation.PopAsync();
                }
                else
                {
                    SetHeight();
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
                else if(ex is OperationCanceledException){ }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, ex.Message, AppResources.MessageBox_ButtonOk);
                }
                ContentVisibility = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Set height of collection view based on items count
        /// </summary>
        public void SetHeight()
        {
            Style style = Application.Current.Resources["TransparentMenuItem"] as Style;
            var height = style.Setters.FirstOrDefault(s => s.Property.PropertyName.Equals("HeightRequest")).Value;
            Thickness padding = (Thickness)style.Setters.FirstOrDefault(s => s.Property.PropertyName.Equals("Padding")).Value;
            CollectionViewHeight = Halls.Count * ((double)height + padding.VerticalThickness) + (Halls.Count - 1) * 20 + 2;
        }

        public async Task SelectHall(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                await Navigation.PushAsync(new HallPage(id));
            }
        }
        
        public async Task NavigateToOptionsPage()
        {
            await Navigation.PushAsync(new OptionsPage());
        }

        /// <summary>
        /// Cancel current token
        /// </summary>
        public void Cancel()
        {
            if (!_cancellationTokenSource.IsCancellationRequested && IsBusy)
            {
                _cancellationTokenSource.Cancel();
            }
        }
        #endregion
    }
}
