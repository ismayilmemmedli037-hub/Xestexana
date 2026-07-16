using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Hospital.Properties;

namespace Hospital.Message
{
    public class EmailService
    {
        private readonly string _email = "ismayilmemmedli037@gmail.com";
        private readonly string _appPassword = "xnxm fqvr zrqq tccz";

        public void SendEmail(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hospital", _email));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_email, _appPassword);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email gonderilmedi: {ex.Message}");
            }
            finally
            {
                if (smtp.IsConnected) smtp.Disconnect(true);
            }
        }

        public void SendNewDoctorApplicationNotification(
            string ad, string soyad, int tecrube, Department sobe, string doctorEmail)
        {
            string subject = "Yeni Hekim Muraciati";
            string body =
                $"Ad Soyad: {ad} {soyad}\n" +
                $"Tecrube: {tecrube} il\n" +
                $"Sobe: {sobe}\n" +
                $"Email: {doctorEmail}";

            SendEmail(_email, subject, body);
        }
    }
}