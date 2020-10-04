using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    class OptionsPageViewModel : BaseViewModel
    {
        public INavigation Navigation;
        public ContentView Popup;
        public StackLayout RadioGroup;

        #region Commands

        public Command OnLabelTapCommand { get; }
        public Command ClearCacheCommand { get; }
        public Command LoadCacheCommand { get; }
        public Command NavigateBackCommand { get; }
        public Command ShowPopupCommand { get; }
        public Command CancelCommand { get; }

        #endregion


        #region Bindings
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

        // Cancel button on language picker
        private string _cancelButton;
        public string CancelButton
        {
            get
            {
                return _cancelButton;
            }

            set
            {
                if (value != _cancelButton)
                {
                    _cancelButton = value;
                }
                OnPropertyChanged(nameof(CancelButton));
            }
        }

        // Is selected english language
        private bool _isSelectedEnglish;
        public bool IsSelectedEnglish
        {
            get
            {
                return _isSelectedEnglish;
            }

            set
            {
                if (value != _isSelectedEnglish)
                {
                    _isSelectedEnglish = value;
                    if (IsVisibleLanguageSelection)
                    {
                        App.Settings.Language = new Language() { CultureInfo = new System.Globalization.CultureInfo("en-US"), LanguageName = "English" };
                        ChangeLanguage();
                    }
                }
                OnPropertyChanged(nameof(IsSelectedEnglish));
            }
        }

        // Is selected russian language
        private bool _isSelectedRussian;
        public bool IsSelectedRussian
        {
            get
            {
                return _isSelectedRussian;
            }

            set
            {
                if (value != _isSelectedRussian)
                {
                    _isSelectedRussian = value;
                    if (IsVisibleLanguageSelection)
                    {
                        App.Settings.Language = new Language() { CultureInfo = new System.Globalization.CultureInfo("ru-RU"), LanguageName = "Русский" };
                        ChangeLanguage();
                    }
                }
                OnPropertyChanged(nameof(IsSelectedRussian));
            }
        }

        // Is selected belorussian language
        private bool _isSelectedBelorussian;
        public bool IsSelectedBelorussian
        {
            get
            {
                return _isSelectedBelorussian;
            }

            set
            {
                if (value != _isSelectedBelorussian)
                {
                    _isSelectedBelorussian = value;
                    if (IsVisibleLanguageSelection)
                    {
                        App.Settings.Language = new Language() { CultureInfo = new System.Globalization.CultureInfo("be-BY"), LanguageName = "Беларуская" };
                        ChangeLanguage();
                    }
                }
                OnPropertyChanged(nameof(IsSelectedBelorussian));
            }
        }

        // Is visible language selection popup
        private bool _isVisibleLanguageSelection = false;
        public bool IsVisibleLanguageSelection
        {
            get
            {
                return _isVisibleLanguageSelection;
            }

            set
            {
                if (value != _isVisibleLanguageSelection)
                {
                    _isVisibleLanguageSelection = value;
                }
                OnPropertyChanged(nameof(IsVisibleLanguageSelection));
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
        private string _language;
        public string Language
        {
            get
            {
                return _language;
            }

            set
            {
                if (value != _language)
                {
                    _language = value;
                    ChangeLanguage();
                }
                OnPropertyChanged(nameof(Language));
            }
        }

        #endregion

        public OptionsPageViewModel(INavigation navigation, ContentView popup, StackLayout radioGroup) 
        {
            Navigation = navigation;
            Popup = popup;
            RadioGroup = radioGroup;
            Language = App.Settings.Language.LanguageName;
            switch (App.Settings.Language.LanguageName)
            {
                case "English":
                    IsSelectedEnglish = true;
                    break;
                case "Русский":
                    IsSelectedRussian = true;
                    break;
                case "Беларуская":
                    IsSelectedBelorussian = true;
                    break;
                default:
                    IsSelectedEnglish = true;
                    break;
            }

            UseCacheIsChecked = App.Settings.UseCache;
            UseOnlyCacheIsSelected = App.Settings.UseOnlyCache;
            CheckForUpdatesIsSelected = App.Settings.CheckForUpdates;
            LocalizePage();
            NavigateBackCommand = new Command(async () => await navigation.PopAsync());
            OnLabelTapCommand = new Command(labelId => OnLabelTap(int.Parse(labelId.ToString())));
            ClearCacheCommand = new Command(async() => await DependencyService.Get<CachingService>().ClearCache());
            LoadCacheCommand = new Command(async() => await LoadCache());
            ShowPopupCommand = new Command(() => ShowPopup());
            CancelCommand = new Command(() => IsVisibleLanguageSelection = false);
        }

        #region Methods

        public void ShowPopup() 
        {
            IsVisibleLanguageSelection = true;
            RadioGroup.AnchorX = 0.5;
            RadioGroup.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => RadioGroup.Scale = f,
                0.2,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => RadioGroup.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(RadioGroup, "radioGroupScaleAnimation", length : 200);
            fadeAnimation.Commit(RadioGroup, "radioGroupFadeAnimation", length : 200);
        }

        /// <summary>
        /// Change Culture by selected language
        /// </summary>
        private void ChangeLanguage()
        {
            Language = App.Settings.Language.LanguageName;
            DependencyService.Get<LocalizationService>().Localize();
            IsVisibleLanguageSelection = false;
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
            UseCacheSectionTitle = AppResources.OptionsPage_CacheSectionTitle;
            LanguageSectionTitle = AppResources.OptionsPage_LanguageSectionTitle;
            CancelButton = AppResources.OptionsPage_CancelButton;
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

        /// <summary>
        /// Load current cache from API 
        /// </summary>
        /// <returns></returns>
        public async Task LoadCache()
        {
            try
            {
                await DependencyService.Get<NetworkService>().LoadCacheAsync();
            }
            catch(Exception ex)
            {
                if(ex is Error er)
                {
                    if(er.ErrorCode == Errors.Info)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleInfo, er.Info, AppResources.MessageBox_ButtonOk);
                    }
                    else if (er.ErrorCode == Errors.Not_found)
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, er.Info, AppResources.MessageBox_ButtonOk);
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, er.Info, AppResources.MessageBox_ButtonOk);
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, ex.Message, AppResources.MessageBox_ButtonOk);
                }
            }
        }
        #endregion
    }
}
