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
        public Command GetHallsCommand { get; }
        public Command SelectHallCommand { get; }
        public ObservableCollection<HallDTO> Halls { get; }

        #endregion

        public MainPageViewModel(INavigation navigation) : base(navigation)
        {
            Navigation = navigation;
            Halls = new ObservableCollection<HallDTO>();
            GetHallsCommand = new Command(async () => await GetHallsAsync());
            SelectHallCommand = new Command(async id => await SelectHallAsync(id?.ToString()));
        }

        #region Methods
        public async Task GetHallsAsync()
        {
            Halls.Clear();
            await LoadAsync(LoadHallsAsync);            
        }

        public async Task LoadHallsAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            var halls = await DependencyService.Get<ContentLoaderService>().LoadHallsAsync(_cancellationTokenSource.Token);
            Halls.Clear();
            foreach (var hall in halls)
            {
                if (string.IsNullOrEmpty(hall.Title))
                {
                    continue;
                }
                Halls.Add(hall);
            }
            if (halls.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.ErrorMessage_InProgress, AppResources.MessageBox_ButtonOk);
                await Navigation.PopAsync();
            }
            else
            {
                SetHeight(Halls.Count);
                ContentVisibility = true;
            }
        }

        
        public async Task SelectHallAsync(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                await Navigation.PushAsync(new HallPage(id));
            }
        }
        
        public async Task NavigateToOptionsPageAsync()
        {
            await Navigation.PushAsync(new OptionsPage());
        }

        #endregion
    }
}
