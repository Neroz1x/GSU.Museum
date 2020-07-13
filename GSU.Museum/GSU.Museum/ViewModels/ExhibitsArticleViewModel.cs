﻿using GSU.Museum.Shared.Data.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace GSU.Museum.Shared.ViewModels
{
    public class ExhibitsArticleViewModel : BaseViewModel
    {
        #region Fields
        public INavigation Navigation;
        public Command NavigateToMainMenuCommand { get; }
        public ObservableCollection<ImageSource> Photos { get; }
        
        public Exhibit Exhibit;

        // Visibility of photo
        private bool _carouselVisibility = false;
        public bool CarouselVisibility
        {
            get
            {
                return _carouselVisibility;
            }

            set
            {
                if (value != _carouselVisibility)
                {
                    _carouselVisibility = value;
                }
                OnPropertyChanged(nameof(CarouselVisibility));
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

        // Title of the page
        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (value != _text)
                {
                    _text = value;
                }
                OnPropertyChanged(nameof(Text));
            }
        }
        #endregion

        public ExhibitsArticleViewModel(Exhibit exhibit, INavigation navigation)
        {
            Navigation = navigation;
            Exhibit = exhibit;
            Photos = new ObservableCollection<ImageSource>();
            NavigateToMainMenuCommand = new Command(async () => await NavigateToMainMenu());
        }

        #region Methods
        public async Task FillPage()
        {
            await Task.Factory.StartNew(() => 
            {
                if(Exhibit.Photos != null && Exhibit.Photos?.Count != 0)
                {
                    Photos.Clear();
                    foreach (var photo in Exhibit.Photos)
                    {
                        Photos.Add(ImageSource.FromStream(() => new MemoryStream(photo)));
                    }
                    CarouselVisibility = true;
                }
                else
                {
                    CarouselVisibility = false;
                }
            });
            Title = Exhibit.Title;
            Text = Exhibit.Text;
        }

        public async Task NavigateToMainMenu()
        {
            await Navigation.PushAsync(new MainPage());
        }
        #endregion
    }
}
