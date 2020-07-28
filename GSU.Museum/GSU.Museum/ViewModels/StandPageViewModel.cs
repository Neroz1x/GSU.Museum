using GSU.Museum.CommonClassLibrary.Models;
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
        public ObservableCollection<ExhibitDTO> Exhibits { get; }

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
            Exhibits = new ObservableCollection<ExhibitDTO>();
            GetExhibitsCommand = new Command(async () => await GetExhibits());
            SelectExhibitCommand = new Command(async Id => await SelectExhibit((string)Id));
        }

        #region Methods
        public async Task GetExhibits()
        {
            ContentVisibility = false;
            IsBusy = true;
            try
            {
                var stand = await DependencyService.Get<ContentLoaderService>().LoadStandAsync(_hallId, _standId);
                Title = $"{stand.Title} - {AppResources.StandPage_Title}";
                Exhibits.Clear();
                foreach (var exhibit in stand.Exhibits)
                {
                    if(string.IsNullOrEmpty(exhibit.Text) || string.IsNullOrEmpty(exhibit.Title) || exhibit.State == false)
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
                    ContentVisibility = true;
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
