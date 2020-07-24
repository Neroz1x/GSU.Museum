using GSU.Museum.Shared.Interfaces;
using NLog;
using NLog.Config;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace GSU.Museum.Shared.Services
{
    public class LogService : ILogService
    {
        public void Initialize(Assembly assembly, string assemblyName)
        {
            string resourcePrefix;
            if (Device.RuntimePlatform == Device.iOS)
            {
                resourcePrefix = "GSU.Museum.iOS";
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                resourcePrefix = "GSU.Museum.Droid";
            }
            else
            {
                throw new Exception("Could not initialize Logger: Unknonw Platform");
            }

            string location = $"{resourcePrefix}.NLog.config";
            Stream stream = assembly.GetManifestResourceStream(location);
            if (stream == null)
            {
                throw new Exception($"The resource '{location}' was not loaded properly.");
            }
            LogManager.Configuration = new XmlLoggingConfiguration(System.Xml.XmlReader.Create(stream), null);
        }
    }
}
