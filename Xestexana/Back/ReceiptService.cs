using Hospital.Properties;

namespace Hospital.Back
{
    public class ReceiptService
    {
        private readonly string _receiptFolder = "Receipts";

        public string GenerateReceipt(User user, Doctor doctor, Appointment appointment)
        {
            if (!Directory.Exists(_receiptFolder))
            {
                Directory.CreateDirectory(_receiptFolder);
            }

            string fileName = $"Cek_{user.Ad}_{user.Soyad}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string fullPath = Path.Combine(_receiptFolder, fileName);

            string content =
                "==================== Cek ====================\n" +
                $"Cek nomresi: {appointment.Id}-{DateTime.Now:yyyyMMddHHmmss}\n" +
                $"Tarix: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n" +
                $"Pasiyent: {user.TamAd}\n" +
                $"Email: {user.Email}\n" +
                $"Telefon: {user.Telefon}\n" +
                $"Sobe: {doctor.Sobe}\n" +
                $"Hekim: {doctor.TamAd}\n" +
                $"İs tecrubesi: {doctor.IsTecrubesi} il\n" +
                $"Qebul tarixi: {appointment.Tarix:dd.MM.yyyy}\n" +
                $"Qebul saatı: {appointment.SaatAraligi}\n" +
                "=====================================================\n";

            File.WriteAllText(fullPath, content);
            return fullPath;
        }
    }
}