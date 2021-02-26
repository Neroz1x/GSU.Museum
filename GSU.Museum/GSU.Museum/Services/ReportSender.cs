using GSU.Museum.Shared.Interfaces;
using GSU.Museum.Shared.Services;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(ReportSender))]
namespace GSU.Museum.Shared.Services
{
    public class ReportSender : IReportSender
    {
        public async Task SendReport()
        {
            if(!DependencyService.Get<NetworkService>().CheckConnection() || !App.Settings.SendReports)
            {
                return;
            }

            MailMessage message = new MailMessage(new MailAddress("GSUMuseum@yandex.by"), new MailAddress("GSUMuseum.sup@yandex.by"))
            {
                Subject = "Bug report from GSUMuseum",
                Body = $"Date: {DateTime.Now}; Platform: {Device.RuntimePlatform}",
            };
            SmtpClient client = new SmtpClient("smtp.yandex.ru")
            {
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("GSUMuseum@yandex.by", "QWEASDzxc1"),
                EnableSsl = true,
            };
            string fileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/logs/file.csv";
            if (File.Exists(fileName))
            {
                using(var fs = File.OpenRead(fileName))
                {
                    message.Attachments.Add(new Attachment(fs, "logs.csv", MediaTypeNames.Application.Octet));
                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
