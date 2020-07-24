﻿using GSU.Museum.CommonClassLibrary.Models;
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
        
        public ExhibitDTO Exhibit;

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
        
        // Visibility of IndivatorView
        private bool _indicatorVisibility = true;
        public bool IndicatorVisibility
        {
            get
            {
                return _indicatorVisibility;
            }

            set
            {
                if (value != _indicatorVisibility)
                {
                    _indicatorVisibility = value;
                }
                OnPropertyChanged(nameof(IndicatorVisibility));
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

        public ExhibitsArticleViewModel(ExhibitDTO exhibit, INavigation navigation)
        {
            Navigation = navigation;
            Exhibit = exhibit;
            Photos = new ObservableCollection<ImageSource>();
            NavigateToMainMenuCommand = new Command(async () => await NavigateToMainMenu());
        }

        #region Methods
        public void FillPage()
        {
            if(Exhibit.Photos?.Count != 0)
            {
                Photos.Clear();
                foreach (var photo in Exhibit.Photos)
                {
                    if(photo?.Photo != null)
                    {
                        Photos.Add(ImageSource.FromStream(() => new MemoryStream(photo.Photo)));
                    }
                }
                CarouselVisibility = true;
                if(Photos.Count == 1)
                {
                    IndicatorVisibility = false;
                }
                else
                {
                    IndicatorVisibility = true;
                }
            }
            else
            {
                CarouselVisibility = false;
            }
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
