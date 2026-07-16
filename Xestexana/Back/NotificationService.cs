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
                $"Kimə      : {toEmail}\n" +
                $"Mövzu     : {subject}\n" +
                $"Mətn      : {body}\n";

            JsonFileService.AppendLine(_logPath, entry);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine();
            Console.WriteLine($"Mail Gonderildi -> {toEmail} | Movzu: {subject}");
            Console.ResetColor();
        }
    }
}