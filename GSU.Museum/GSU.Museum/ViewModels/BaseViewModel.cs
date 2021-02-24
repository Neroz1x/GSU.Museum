using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Resources;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public Command NavigateToHomePageCommand { get; }
        public Command NavigateBackCommand { get; }

        public INavigation Navigation;


        // Indicates whether task was canceled by user
        protected bool _isCanceled = false;

        protected CancellationTokenSource _cancellationTokenSource;

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
        private bool _isBusy = false;
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
        
        public BaseViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NavigateToHomePageCommand = new Command(() => App.Current.MainPage = new NavigationPage(new HomePage()));
            NavigateBackCommand = new Command(async () => await navigation.PopAsync());
        }

        /// <summary>
        /// Set height of collection view based on items count
        /// </summary>
        public void SetHeight(int itemsCount)
        {
            Style style = Application.Current.Resources["TransparentMenuItem"] as Style;
            var height = style.Setters.FirstOrDefault(s => s.Property.PropertyName.Equals("HeightRequest")).Value;
            Thickness padding = (Thickness)style.Setters.FirstOrDefault(s => s.Property.PropertyName.Equals("Padding")).Value;
            CollectionViewHeight = itemsCount * ((double)height + padding.VerticalThickness) + (itemsCount - 1) * 20 + 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public async Task LoadAsync(Func<Task> func)
        {
            ContentVisibility = false;
            IsBusy = true;

            try
            {
                await func();
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
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                }
                else if (ex is HttpRequestException || ex is WebException)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
                }
                else if (ex is OperationCanceledException)
                {
                    if (!_isCanceled)
                    {
                        await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
                        await App.Current.MainPage.Navigation.PopAsync();
                    }
                }
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
        /// Cancel current token
        /// </summary>
        public void Cancel()
        {
            if (!_cancellationTokenSource.IsCancellationRequested && IsBusy)
            {
                _isCanceled = true;
                _cancellationTokenSource.Cancel();
            }
        }
    }
}