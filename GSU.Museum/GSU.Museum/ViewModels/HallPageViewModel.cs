using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Stand> Stands { get; }

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
        #endregion

        public HallPageViewModel(string hallId, INavigation navigation)
        {
            Navigation = navigation;
            _hallId = hallId;
            Stands = new ObservableCollection<Stand>();
            GetStandsCommand = new Command(async () => await GetStands());
            SelectStandCommand = new Command(async id => await SelectStand(id.ToString()));
        }

        #region Methods
        public async Task GetStands()
        {
            try
            {
                IsBusy = true;
                var hall = await DependencyService.Get<ContentLoaderService>().LoadHall(_hallId);
                Title = hall.Title;
                Stands.Clear();
                foreach (var stand in hall.Stands)
                {
                    Stands.Add(stand);
                }
            }
            catch (Exception ex)
            {
                if (ex is Error error)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", error.Info, "Ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
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
