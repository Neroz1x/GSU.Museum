using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    public class StandPageViewModel : BaseViewModel
    {
        #region Fields
        public INavigation Navigation;
        public Command GetExhibitsCommand { get; }
        public Command SelectExhibitCommand { get; }
        public ObservableCollection<Exhibit> Exhibits { get; }

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

        // Visibility of photo
        private bool _visibility = false;
        public bool PhotoVisibility
        {
            get
            {
                return _visibility;
            }

            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                }
                OnPropertyChanged(nameof(PhotoVisibility));
            }
        }

        // Photo of stand
        private ImageSource _photo;
        public ImageSource Photo
        {
            get
            {
                return _photo;
            }

            set
            {
                if (value != _photo)
                {
                    _photo = value;
                }
                OnPropertyChanged(nameof(Photo));
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
        private string _standId;
        #endregion

        public StandPageViewModel(string hallId, string id, INavigation navigation)
        {
            _hallId = hallId;
            _standId = id;
            Navigation = navigation;
            Exhibits = new ObservableCollection<Exhibit>();
            GetExhibitsCommand = new Command(async () => await GetExhibits());
            SelectExhibitCommand = new Command(async Id => await SelectExhibit((string)Id));
        }

        #region Methods
        public async Task GetExhibits()
        {
            IsBusy = true;
            try
            {
                var stand = await DependencyService.Get<ContentLoaderService>().LoadStandAsync(_hallId, _standId);
                Title = stand.Title;
                Exhibits.Clear();
                foreach (var exhibit in stand.Exhibits)
                {
                    if(string.IsNullOrEmpty(exhibit.Text) || string.IsNullOrEmpty(exhibit.Title) || exhibit.State == false)
                    {
                        continue;
                    }
                    Exhibits.Add(exhibit);
                }
                if(stand.Photo?.Photo != null)
                {
                    Photo = ImageSource.FromStream(() => new MemoryStream(stand.Photo.Photo));
                    PhotoVisibility = true;
                }
                else
                {
                    PhotoVisibility = false;
                }
                if (Exhibits.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_InProgress, AppResources.MessageBox_ButtonOk);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                if (ex is Error error)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, error.Info, AppResources.MessageBox_ButtonOk);
                    await Navigation.PopAsync();
                }
                else if (ex is HttpRequestException || ex is WebException)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
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

        public async Task SelectExhibit(string id)
        {
            try
            {
                IsBusy = true;
                var exhibit = await DependencyService.Get<ContentLoaderService>().LoadExhibitAsync(_hallId, _standId, id);
                await Navigation.PushAsync(new ExhibitsArticle(exhibit));
            }
            catch(Exception ex)
            {
                if (ex is Error error)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, error.Info, AppResources.MessageBox_ButtonOk);
                }
                else if (ex is HttpRequestException || ex is WebException)
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_ServerIsNotResponse, AppResources.MessageBox_ButtonOk);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, ex.Message, AppResources.MessageBox_ButtonOk);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
