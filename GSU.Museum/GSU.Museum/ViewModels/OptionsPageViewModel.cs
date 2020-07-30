using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    class OptionsPageViewModel : BaseViewModel
    {
        #region Fields
        public Command OnLabelTapCommand { get; }
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

        // Use cache option
        private string _useCacheLabel;
        public string UseCacheLabel
        {
            get
            {
                return _useCacheLabel;
            }

            set
            {
                if (value != _useCacheLabel)
                {
                    _useCacheLabel = value;
                }
                OnPropertyChanged(nameof(UseCacheLabel));
            }
        }

        // Use cache description
        private string _useCacheDescriptionLabel;
        public string UseCacheDescriptionLabel
        {
            get
            {
                return _useCacheDescriptionLabel;
            }

            set
            {
                if (value != _useCacheDescriptionLabel)
                {
                    _useCacheDescriptionLabel = value;
                }
                OnPropertyChanged(nameof(UseCacheDescriptionLabel));
            }
        }

        // Use cache section title
        private string _useCacheSectionTitle;
        public string UseCacheSectionTitle
        {
            get
            {
                return _useCacheSectionTitle;
            }

            set
            {
                if (value != _useCacheSectionTitle)
                {
                    _useCacheSectionTitle = value;
                }
                OnPropertyChanged(nameof(UseCacheSectionTitle));
            }
        }

        // Language section title
        private string _languageSectionTitle;
        public string LanguageSectionTitle
        {
            get
            {
                return _languageSectionTitle;
            }

            set
            {
                if (value != _languageSectionTitle)
                {
                    _languageSectionTitle = value;
                }
                OnPropertyChanged(nameof(LanguageSectionTitle));
            }
        }

        // Check for updates label
        private string _checkForUpdatesLabel;
        public string CheckForUpdatesLabel
        {
            get
            {
                return _checkForUpdatesLabel;
            }

            set
            {
                if (value != _checkForUpdatesLabel)
                {
                    _checkForUpdatesLabel = value;
                }
                OnPropertyChanged(nameof(CheckForUpdatesLabel));
            }
        }

        // Check for updates label description
        private string _checkForUpdatesDescriptionLabel;
        public string CheckForUpdatesDescriptionLabel
        {
            get
            {
                return _checkForUpdatesDescriptionLabel;
            }

            set
            {
                if (value != _checkForUpdatesDescriptionLabel)
                {
                    _checkForUpdatesDescriptionLabel = value;
                }
                OnPropertyChanged(nameof(CheckForUpdatesDescriptionLabel));
            }
        }

        // Use only cache label
        private string _useOnlyCacheLabel;
        public string UseOnlyCacheLabel
        {
            get
            {
                return _useOnlyCacheLabel;
            }

            set
            {
                if (value != _useOnlyCacheLabel)
                {
                    _useOnlyCacheLabel = value;
                }
                OnPropertyChanged(nameof(UseOnlyCacheLabel));
            }
        }

        // Use only cache label description
        private string _useOnlyCacheDescriptionLabel;
        public string UseOnlyCacheDescriptionLabel
        {
            get
            {
                return _useOnlyCacheDescriptionLabel;
            }

            set
            {
                if (value != _useOnlyCacheDescriptionLabel)
                {
                    _useOnlyCacheDescriptionLabel = value;
                }
                OnPropertyChanged(nameof(UseOnlyCacheDescriptionLabel));
            }
        }

        // Use cache is chekced
        private bool _useCacheIsChecked;
        public bool UseCacheIsChecked
        {
            get
            {
                return _useCacheIsChecked;
            }

            set
            {
                if (value != _useCacheIsChecked)
                {
                    _useCacheIsChecked = value;
                    App.Settings.UseCache = value;
                }
                OnPropertyChanged(nameof(UseCacheIsChecked));
            }
        }

        // Check for updates is selected
        private bool _checkForUpdatesIsSelected;
        public bool CheckForUpdatesIsSelected
        {
            get
            {
                return _checkForUpdatesIsSelected;
            }

            set
            {
                if (value != _checkForUpdatesIsSelected)
                {
                    _checkForUpdatesIsSelected = value;
                    App.Settings.CheckForUpdates = value;
                    App.Settings.UseOnlyCache = !value;
                }
                OnPropertyChanged(nameof(CheckForUpdatesIsSelected));
            }
        }

        // Use only cache is selected
        private bool _useOnlyCacheIsSelected;
        public bool UseOnlyCacheIsSelected
        {
            get
            {
                return _useOnlyCacheIsSelected;
            }

            set
            {
                if (value != _useOnlyCacheIsSelected)
                {
                    _useOnlyCacheIsSelected = value;
                    App.Settings.CheckForUpdates = !value;
                    App.Settings.UseOnlyCache = value;
                }
                OnPropertyChanged(nameof(UseOnlyCacheIsSelected));
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
            UseCacheIsChecked = App.Settings.UseCache;
            UseOnlyCacheIsSelected = App.Settings.UseOnlyCache;
            CheckForUpdatesIsSelected = App.Settings.CheckForUpdates;
            LocalizePage();
            OnLabelTapCommand = new Command(labelId => OnLabelTap(int.Parse(labelId.ToString())));
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
            UseCacheLabel = AppResources.OptionsPage_UseCacheLabel;
            UseCacheDescriptionLabel = AppResources.OptionsPage_UseCacheDescriptionLabel;
            CheckForUpdatesLabel = AppResources.OptionsPage_CheckForUpdatesLabel;
            CheckForUpdatesDescriptionLabel = AppResources.OptionsPage_CheckForUpdatesDescriptionLabel;
            UseOnlyCacheLabel = AppResources.OptionsPage_UseOnlyCacheLabel;
            UseOnlyCacheDescriptionLabel = AppResources.OptionsPage_UseOnlyCacheDescriptionLabel;
        }

        /// <summary>
        /// Performs when detects tap on label
        /// </summary>
        /// <param name="labelId">0 if tapped use cahce checkbox; 1 - chech for updates; 2 - use only cahce</param>
        private void OnLabelTap(int labelId)
        {
            switch (labelId)
            {
                case 0:
                    UseCacheIsChecked = !UseCacheIsChecked;
                    break;
                case 1:
                    if (!CheckForUpdatesIsSelected)
                    {
                        CheckForUpdatesIsSelected = true;
                    }
                    break;
                case 2:
                    if (!UseOnlyCacheIsSelected)
                    {
                        UseOnlyCacheIsSelected = true;
                    }
                    break;
            }
        }
        #endregion
    }
}
