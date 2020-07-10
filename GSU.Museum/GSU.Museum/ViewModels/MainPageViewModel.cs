﻿using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Pages;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Hall> Halls { get; }

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
            Halls = new ObservableCollection<Hall>();
            GetHallsCommand = new Command(async () => await GetHalls());
            SelectHallCommand = new Command(async Id => await SelectHall((string)Id));
        }

        #region Methods
        public async Task GetHalls()
        {
            ReloadButtonVisibility = false;
            ContentVisibility = true;
            IsBusy = true;
            try
            {
                var halls = await DependencyService.Get<ContentLoaderService>().LoadHalls();
                Halls.Clear();
                foreach (var hall in halls)
                {
                    Halls.Add(hall);
                }
            }
            catch(Exception ex)
            {
                if (ex is Error error)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", error.Info, "Ok");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
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
        #endregion
    }
}