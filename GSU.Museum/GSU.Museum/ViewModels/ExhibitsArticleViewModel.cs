using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Pages;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Forms;


namespace GSU.Museum.Shared.ViewModels
{
    public class ExhibitsArticleViewModel : BaseViewModel
    {
        #region Fields
        public INavigation Navigation;
        public Command NavigateToHomePageCommand { get; }
        public Command NavigateToHallSelectionPageCommand { get; }
        public Command NavigateToStandSelectionPageCommand { get; }
        public Command NavigateToExhibitSelectionPageCommand { get; }
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exhibit">Exhibit to display</param>
        /// <param name="navigation">Instance of navigation</param>
        /// <param name="hallId">Id of the hall</param>
        /// <param name="standId">Id of the stand</param>
        public ExhibitsArticleViewModel(ExhibitDTO exhibit, INavigation navigation, string hallId, string standId)
        {
            Navigation = navigation;
            Exhibit = exhibit;
            Photos = new ObservableCollection<ImageSource>();
            NavigateToHomePageCommand = new Command(() => App.Current.MainPage = new NavigationPage(new HomePage()));
            NavigateToHallSelectionPageCommand = new Command(async () => await Navigation.PushAsync(new MainPage()));
            NavigateToStandSelectionPageCommand = new Command(async () => await Navigation.PushAsync(new HallPage(hallId)));
            NavigateToExhibitSelectionPageCommand = new Command(async () => await Navigation.PushAsync(new StandPage(hallId, standId)));
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
        #endregion
    }
}
