using System.Dynamic;
using System.Globalization;
using System.Threading;

namespace GSU.Museum.Shared.Data.Models
{
    public class Settings
    {
        public Language Language { get; set; }
        public bool UseCache { get; set; }
        public bool CheckForUpdates { get; set; }
        public bool UseOnlyCache { get; set; }
        public Settings()
        {
            string language;
            CultureInfo cultureInfo;
            switch (Thread.CurrentThread.CurrentUICulture.Name)
            {
                case "ru-RU":
                    language = "Русский";
                    cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    break;
                case "en-US":
                    language = "English";
                    cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    break;
                case "be-BY":
                    language = "Беларуская";
                    cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    break;
                default:
                    language = "English";
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    break;
            }
            
            Language = new Language() { CultureInfo = cultureInfo, LanguageName = language};
            UseCache = true;
            CheckForUpdates = true;
        }
    }
}
