using Hospital.Back;
using static Hospital.Info.ConsoleInfo;
using static Hospital.Info.InputInfo;
using Hospital.Properties;

namespace Hospital.Menus
{
    public class DoctorMenu
    {
        public static void DoctorRegisterFlow()
        {
            PrintHeader("Hekimin Is Qeydiyyati");
            Console.Write("Adiniz: ");
            string ad = Console.ReadLine() ?? "";
            Console.Write("Soyadiniz: ");
            string soyad = Console.ReadLine() ?? "";
            int tecrube = ReadInt("Is tecrubeniz il daxil edin: ");
            var departments = Enum.GetValues<Department>();
            Console.WriteLine();
            Console.WriteLine("Sobe secin:");
            for (int i = 0; i < departments.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i]}");
            }
            int deptIndex = ReadInt("Seciminiz: ");
            while (deptIndex < 1 || deptIndex > departments.Length)
            {
                PrintError("Yanlis secim");
                deptIndex = ReadInt("Seciminiz: ");
            }
            Department sobe = departments[deptIndex - 1];

            string email;
            while (true)
            {
                Console.Write("Email: ");
                email = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(email))
                {
                    PrintError("Email bos ola bilmez");
                    continue;
                }
                if (AppServices.DoctorService.EmailExists(email))
                {
                    PrintError("Bu email artiq istifade olunub");
                    continue;
                }
                break;
            }

            Console.Write("Sifre: ");
            string password = Console.ReadLine() ?? "";
            AppServices.DoctorService.ApplyForJob(ad, soyad, tecrube, sobe, email, password);
            AppServices.EmailService.SendNewDoctorApplicationNotification(ad, soyad, tecrube, sobe, email);
            PrintSuccess($"Muraciatiniz qebul olundu,CV admin terefinden baxilacaq. Tesdiqlendikden sonra login edersiz");
            Pause();
        }

        public static void DoctorLoginFlow()
        {
            PrintHeader("Hekim Login");
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Sifre: ");
            string password = Console.ReadLine() ?? "";
            var doctor = AppServices.DoctorService.Login(email, password);
            if (doctor == null)
            {
                PrintError("Email ve ya sifre yanlisdi");
                Pause();
                return;
            }

            switch (doctor.Status)
            {
                case DoctorStatus.Pending:
                    PrintError("Muraciatiniz hele admin terefinden baxilir, Zehmet olmasa gozleyin");
                    Pause();
                    return;
                case DoctorStatus.Rejected:
                    PrintError("Muraciatiniz redd edilidi");
                    Pause();
                    return;
            }

            PrintHeader($"Xos Geldiniz {doctor.TamAd}");
            Console.WriteLine($"Sobe: {doctor.Sobe}");
            Console.WriteLine($"Is tecrubesi: {doctor.IsTecrubesi} il");

            var reservedSlots = AppServices.AppointmentService.GetByDoctor(doctor.Id);
            Console.WriteLine();
            Console.WriteLine($"Rezerv olunmus qebulariniz ({reservedSlots.Count}):");
            foreach (var slot in reservedSlots.OrderBy(s => s.Tarix))
            {
                Console.WriteLine($" - {slot.Tarix:dd.MM.yyyy} {slot.SaatAraligi}  |  Pasiyent: {slot.UserTamAd}");
            }

            Pause();
        }
    }
}
