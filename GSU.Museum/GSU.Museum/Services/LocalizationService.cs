using GSU.Museum.CommonClassLibrary.Constants;
using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Services;
using System.Globalization;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalizationService))]
namespace GSU.Museum.Shared.Services
{
    public class LocalizationService : ILocalizationService
    {
        public void Localize()
        {
            switch (App.Settings.Language.CultureInfo.Name)
            {
                case LanguageConstants.LanguageFullRu:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals(LanguageConstants.LanguageFullRu))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(LanguageConstants.LanguageFullRu);
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullRu);
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageConstants.LanguageFullRu);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullRu);
                    }
                    break;
                case LanguageConstants.LanguageFullEn:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals(LanguageConstants.LanguageFullEn))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                    }
                    break;
                case LanguageConstants.LanguageFullBy:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals(LanguageConstants.LanguageFullBy))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(LanguageConstants.LanguageFullBy);
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullBy);
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageConstants.LanguageFullBy);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullBy);
                    }
                    break;
                default:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals(LanguageConstants.LanguageFullEn))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        Thread.CurrentThread.CurrentCulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageConstants.LanguageFullEn);
                    }
                    break;
            }
        }
    }
}
