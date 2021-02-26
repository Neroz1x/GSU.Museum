using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.CommonClassLibrary.Enums;
using GSU.Museum.CommonClassLibrary.Models;
using GSU.Museum.Shared.Data.Models;
using GSU.Museum.Shared.Resources;
using GSU.Museum.Shared.Services;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GSU.Museum.Shared.ViewModels
{
    class OptionsPageViewModel : BaseViewModel
    {
        public StackLayout RadioGroup;
        public StackLayout CacheGroup;

        #region Commands

        public Command OnLabelTapCommand { get; }
        public Command ClearCacheCommand { get; }
        public Command LoadCacheCommand { get; }
        public Command ShowLanguagePopupCommand { get; }
        public Command ShowCachePopupCommand { get; }
        public Command CancelLanguageSelectionCommand { get; }
        public Command CancelCacheSelectionCommand { get; }

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

        // Cancel button
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

        // Select cache language button
        private string _selectCacheButton;
        public string SelectCacheButton
        {
            get
            {
                return _selectCacheButton;
            }

            set
            {
                if (value != _selectCacheButton)
                {
                    _selectCacheButton = value;
                }
                OnPropertyChanged(nameof(SelectCacheButton));
            }
        }
        
        // Select cache language label
        private string _selectCacheLabel;
        public string SelectCacheLabel
        {
            get
            {
                return _selectCacheLabel;
            }

            set
            {
                if (value != _selectCacheLabel)
                {
                    _selectCacheLabel = value;
                }
                OnPropertyChanged(nameof(SelectCacheLabel));
            }
        }

        // Clear cache button
        private string _clearCacheButton;
        public string ClearCacheButton
        {
            get
            {
                return _clearCacheButton;
            }

            set
            {
                if (value != _clearCacheButton)
                {
                    _clearCacheButton = value;
                }
                OnPropertyChanged(nameof(ClearCacheButton));
            }
        }

        // Download button on cache picker
        private string _downloadButton;
        public string DownloadButton
        {
            get
            {
                return _downloadButton;
            }

            set
            {
                if (value != _downloadButton)
                {
                    _downloadButton = value;
                }
                OnPropertyChanged(nameof(DownloadButton));
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

        // Is selected english language
        private bool _isSelectedEnglishCache;
        public bool IsSelectedEnglishCache
        {
            get
            {
                return _isSelectedEnglishCache;
            }

            set
            {
                if (value != _isSelectedEnglishCache)
                {
                    _isSelectedEnglishCache = value;
                }
                OnPropertyChanged(nameof(IsSelectedEnglishCache));
            }
        }

        // Is selected russian language
        private bool _isSelectedRussianCache;
        public bool IsSelectedRussianCache
        {
            get
            {
                return _isSelectedRussianCache;
            }

            set
            {
                if (value != _isSelectedRussianCache)
                {
                    _isSelectedRussianCache = value;
                }
                OnPropertyChanged(nameof(IsSelectedRussianCache));
            }
        }

        // Is selected belorussian language
        private bool _isSelectedBelorussianCache;
        public bool IsSelectedBelorussianCache
        {
            get
            {
                return _isSelectedBelorussianCache;
            }

            set
            {
                if (value != _isSelectedBelorussianCache)
                {
                    _isSelectedBelorussianCache = value;
                }
                OnPropertyChanged(nameof(IsSelectedBelorussianCache));
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

        // Is visible language selection popup
        private bool _isVisibleCacheSelection = false;
        public bool IsVisibleCacheSelection
        {
            get
            {
                return _isVisibleCacheSelection;
            }

            set
            {
                if (value != _isVisibleCacheSelection)
                {
                    _isVisibleCacheSelection = value;
                }
                OnPropertyChanged(nameof(IsVisibleCacheSelection));
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

        // Downloading status
        private string _statusText;
        public string StatusText
        {
            get
            {
                return _statusText;
            }

            set
            {
                if (value != _statusText)
                {
                    _statusText = value;
                }
                OnPropertyChanged(nameof(StatusText));
            }
        }

        // Downloading status
        private string _downloadingStatus;
        public string DownloadingStatus
        {
            get
            {
                return _downloadingStatus;
            }

            set
            {
                if (value != _downloadingStatus)
                {
                    _downloadingStatus = value;
                }
                OnPropertyChanged(nameof(DownloadingStatus));
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

        // Reports section title
        private string _reportsSectionTitle;
        public string ReportsSectionTitle
        {
            get
            {
                return _reportsSectionTitle;
            }

            set
            {
                if (value != _reportsSectionTitle)
                {
                    _reportsSectionTitle = value;
                }
                OnPropertyChanged(nameof(ReportsSectionTitle));
            }
        }

        // Send reports label
        private string _sendReportsLabel;
        public string SendReportsLabel
        {
            get
            {
                return _sendReportsLabel;
            }

            set
            {
                if (value != _sendReportsLabel)
                {
                    _sendReportsLabel = value;
                }
                OnPropertyChanged(nameof(SendReportsLabel));
            }
        }

        // Send reports description label
        private string _sendReportsDescriptionLabel;
        public string SendReportsDescriptionLabel
        {
            get
            {
                return _sendReportsDescriptionLabel;
            }

            set
            {
                if (value != _sendReportsDescriptionLabel)
                {
                    _sendReportsDescriptionLabel = value;
                }
                OnPropertyChanged(nameof(SendReportsDescriptionLabel));
            }
        }

        // Send reports description label
        private bool _sendReportsIsChecked;
        public bool SendReportsIsChecked
        {
            get
            {
                return _sendReportsIsChecked;
            }

            set
            {
                if (value != _sendReportsIsChecked)
                {
                    _sendReportsIsChecked = value;
                    App.Settings.SendReports = value;
                }
                OnPropertyChanged(nameof(SendReportsIsChecked));
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

        public OptionsPageViewModel(INavigation navigation, StackLayout radioGroup, StackLayout cacheGroup) 
            : base(navigation)
        {
            RadioGroup = radioGroup;
            CacheGroup = cacheGroup;
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
            SendReportsIsChecked = App.Settings.SendReports;
            LocalizePage();
            OnLabelTapCommand = new Command(labelId => OnLabelTap(int.Parse(labelId.ToString())));
            ClearCacheCommand = new Command(async() => await DependencyService.Get<CachingService>().ClearCache());
            LoadCacheCommand = new Command(async() => await LoadCacheAsync());
            ShowLanguagePopupCommand = new Command(() => ShowLanguageSelectionPopup());
            ShowCachePopupCommand = new Command(() => ShowCachePopup());
            CancelLanguageSelectionCommand = new Command(() => IsVisibleLanguageSelection = false);
            CancelCacheSelectionCommand = new Command(() => IsVisibleCacheSelection = false);
        }

        #region Methods

        public void ShowLanguageSelectionPopup() 
        {
            IsVisibleLanguageSelection = true;
            RadioGroup.AnchorX = 0.5;
            RadioGroup.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => RadioGroup.Scale = f,
                0.7,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => RadioGroup.Opacity = f,
                0.9,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(RadioGroup, "radioGroupScaleAnimation", length : 250, rate : 8);
            fadeAnimation.Commit(RadioGroup, "radioGroupFadeAnimation", length : 100, rate : 8);
        }

        public void ShowCachePopup()
        {
            IsVisibleCacheSelection = true;
            CacheGroup.AnchorX = 0.5;
            CacheGroup.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => CacheGroup.Scale = f,
                0.7,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => CacheGroup.Opacity = f,
                0.9,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(CacheGroup, "radioGroupScaleAnimation", length: 250, rate: 8);
            fadeAnimation.Commit(CacheGroup, "radioGroupFadeAnimation", length: 100, rate: 8);
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
            ClearCacheButton = AppResources.OptionsPage_ClearCacheButton;
            DownloadButton = AppResources.OptionsPage_DownloadButton;
            SelectCacheButton = AppResources.OptionsPage_SelectCacheLanguageButton;
            SelectCacheLabel = AppResources.OptionsPage_SelectCacheLanguageLabel;
            SendReportsDescriptionLabel = AppResources.OptionsPage_SendReportsDescriptionLabel;
            SendReportsLabel = AppResources.OptionsPage_SendReportsLabel;
            ReportsSectionTitle = AppResources.OptionsPage_ReportsSectionLabel;
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
                case 3:
                    SendReportsIsChecked = !SendReportsIsChecked;
                    break;
            }
        }

        /// <summary>
        /// Load current cache from API 
        /// </summary>
        /// <returns></returns>
        public async Task LoadCacheAsync()
        {
            try
            {
                // Setting selected languages count
                int languagesCount = BoolToInt(IsSelectedBelorussianCache) + BoolToInt(IsSelectedEnglishCache) + BoolToInt(IsSelectedRussianCache);
                
                if(languagesCount == 0)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleAlert, AppResources.MessageBox_NoSelectedLanguages,
                        AppResources.MessageBox_ButtonOk);
                    return;
                }

                // Hidding cache language selection
                IsVisibleCacheSelection = false;

                IsBusy = true;
                
                // Setting up web client
                var client = DependencyService.Get<NetworkService>().GetWebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    (sender, @event)=>
                    {
                        DownloadingStatus = $"{@event.ProgressPercentage} %";
                    });

                // Downloading and saving photos
                StatusText = AppResources.DownloadingText_DownloadingPhotos;
                var versionKey = "v_photos";
                var stream = new MemoryStream(await client.DownloadDataTaskAsync(
                    await DependencyService.Get<CacheLoadingService>().GetUrlAsync()));
                DownloadingStatus = AppResources.DownloadingText_Saving;
                await DependencyService.Get<CacheLoadingService>().WriteCacheAsync(stream, versionKey, "photo");
                
                // Downloading and saving languages
                if (IsSelectedBelorussianCache)
                {
                    await LoadLanguageCache(LanguageConstants.LanguageBy, client);
                }
                if (IsSelectedEnglishCache)
                {
                    await LoadLanguageCache(LanguageConstants.LanguageEn, client);
                }
                if (IsSelectedRussianCache)
                {
                    await LoadLanguageCache(LanguageConstants.LanguageRu, client);
                }
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
                else if(ex is OperationCanceledException)
                {
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.MessageBox_TitleError, ex.Message, AppResources.MessageBox_ButtonOk);
                }
            }
            finally
            {
                StatusText = string.Empty;
                DownloadingStatus = string.Empty;
                IsBusy = false;
            }
        }

        private async Task LoadLanguageCache(string language, WebClient client)
        {
            var versionKey = $"v_{language}";
            StatusText = AppResources.DownloadingText_DownloadingText;
            var stream = new MemoryStream(await client.DownloadDataTaskAsync(
                await DependencyService.Get<CacheLoadingService>().GetUrlAsync(language)));
            await DependencyService.Get<CacheLoadingService>().WriteCacheAsync(stream, versionKey, language);
        }

        private int BoolToInt(bool param)
        {
            if (param)
            {
                return 1;
            }
            return 0;
        }

        #endregion
    }
}
