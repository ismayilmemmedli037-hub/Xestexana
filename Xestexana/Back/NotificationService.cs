namespace Hospital.Back
{
    public class NotificationService
    {
        private readonly string _logPath = Path.Combine("Data", "notifications.log");

        public void SendEmail(string toEmail, string subject, string body)
        {
            string entry =
                $"--------------------------------------------------\n" +
                $"Tarix     : {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n" +
                $"Kime      : {toEmail}\n" +
                $"Movzu     : {subject}\n" +
                $"Metn      : {body}\n";

            JsonFileService.AppendLine(_logPath, entry);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.ResetColor();
        }
    }
}