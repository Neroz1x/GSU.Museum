using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    class OptionsPageViewModel : BaseViewModel
    {
        #region Fields

        public INavigation Navigation;
        public ObservableCollection<Language> Languages { get; }

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

        // Language option
        private string _languageLabel;
        public string LanguageLabel
        {
            get
            {
                return _languageLabel;
            }

            set
            {
                if (value != _languageLabel)
                {
                    _languageLabel = value;
                }
                OnPropertyChanged(nameof(LanguageLabel));
            }
        }

        // Language option description
        private string _languageDescriptionLabel;
        public string LanguageDescriptionLabel
        {
            get
            {
                return _languageDescriptionLabel;
            }

            set
            {
                if (value != _languageDescriptionLabel)
                {
                    _languageDescriptionLabel = value;
                }
                OnPropertyChanged(nameof(LanguageDescriptionLabel));
            }
        }

        // Title of the page
        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }

            set
            {
                if (value != _selectedLanguage)
                {
                    _selectedLanguage = value;
                    ChangeLanguage();
                }
                OnPropertyChanged(nameof(SelectedLanguage));
            }
        }

        #endregion

        public OptionsPageViewModel(INavigation navigation) 
        {
            Navigation = navigation;
            Languages = new ObservableCollection<Language>();
            Languages.Add(new Language() { CultureInfo = new System.Globalization.CultureInfo("en-US"), LanguageName = "English" });
            Languages.Add(new Language() { CultureInfo = new System.Globalization.CultureInfo("ru-RU"), LanguageName = "Русский" });
            Languages.Add(new Language() { CultureInfo = new System.Globalization.CultureInfo("be-BY"), LanguageName = "Беларуская" });
            switch (App.Settings.Language.LanguageName)
            {
                case "English":
                    SelectedLanguage = Languages.ElementAt(0);
                    break;
                case "Русский":
                    SelectedLanguage = Languages.ElementAt(1);
                    break;
                case "Беларуская":
                    SelectedLanguage = Languages.ElementAt(2);
                    break;
                default:
                    SelectedLanguage = Languages.ElementAt(0);
                    break;
            }
            LocalizePage();
        }

        #region Methods

        /// <summary>
        /// Change Culture by selected language
        /// </summary>
        private void ChangeLanguage()
        {
            App.Settings.Language = SelectedLanguage;
            DependencyService.Get<LocalizationService>().Localize();
            LocalizePage();
        }

        /// <summary>
        /// Localize page content
        /// </summary>
        public void LocalizePage()
        {
            Title = AppResources.OptionsPage_Title;
            LanguageLabel = AppResources.OptionsPage_Language;
            LanguageDescriptionLabel = AppResources.OptionsPage_LanguageDescription;
        }
        #endregion
    }
}
