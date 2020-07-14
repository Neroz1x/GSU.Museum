using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Services;
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
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
                    }
                    break;
                case "en-US":
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("en-US"))
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    }
                    break;
                case "be-BY":
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("be-BY"))
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("be-BY");
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("be-BY");
                    }
                    break;
                default:
                    if (!Thread.CurrentThread.CurrentUICulture.Name.Equals("en-US"))
                    {
                        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                        Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    }
                    break;
            }
        }
    }
}
