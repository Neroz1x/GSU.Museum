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
                case "ru-RU":
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("ru-RU"))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
                    }
                    break;
                case "en-US":
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("en-US"))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    }
                    break;
                case "be-BY":
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("be-BY"))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("be-BY");
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("be-BY");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("be-BY");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("be-BY");
                    }
                    break;
                default:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("en-US"))
                    {
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    }
                    break;
            }
        }
    }
}
