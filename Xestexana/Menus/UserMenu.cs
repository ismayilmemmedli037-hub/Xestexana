using Hospital.Back;
using static Hospital.Info.ConsoleInfo;
using Hospital.Properties;

namespace Hospital.Menus
{
    public class UserMenu
    {
        private static readonly AppointmentService _appointmentService = new();
        private static readonly DoctorService _doctorService = new(_appointmentService);
        private static readonly UserService _userService = new();
        private static readonly ReceiptService _receiptService = new();
        private static readonly NotificationService _notificationService = new();
        public static void UserRegisterFlow()
        {
            PrintHeader("User Qeydiyyati");
            Console.Write("Adiniz: ");
            string ad = Console.ReadLine() ?? "";
            Console.Write("Soyadiniz: ");
            string soyad = Console.ReadLine() ?? "";
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
                if (_userService.EmailExists(email))
                {
                    PrintError("Bu email qeydiyyatdan kecib, Basqa email daxil edin");
                    continue;
                }
                break;
            }

            Console.Write("Telefon: ");
            string telefon = Console.ReadLine() ?? "";
            Console.Write("Sifre: ");
            string password = Console.ReadLine() ?? "";
            _userService.Register(ad, soyad, email, telefon, password);

            PrintSuccess($"Tebrikler {ad} {soyad} qeydiyyat tamamlandi. Indi giris edin");
            Pause();
        }

        public static void UserLoginFlow()
        {
            PrintHeader("User Login");
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Sifre: ");
            string password = Console.ReadLine() ?? "";
            var user = _userService.Login(email, password);
            if (user == null)
            {
                PrintError("Email ve ya sifre yanlisdi");
                Pause();
                return;
            }

            PrintSuccess($"Xos geldiniz {user.TamAd}");
            BookingFlow(user);
        }

        private static void BookingFlow(User user)
        {
            var departments = Enum.GetValues<Department>();
            int backOption = departments.Length + 1;

            PrintHeader("Sobe Secimi");
            for (int i = 0; i < departments.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {departments[i]}");
            }
            Console.WriteLine($"{backOption}. Geri");
            Console.WriteLine();
            Console.Write("Seciminiz: ");
            string? input = Console.ReadLine();

            if (input == backOption.ToString())
            {
                return;
            }

            if (!int.TryParse(input, out int deptIndex) || deptIndex < 1 || deptIndex > departments.Length)
            {
                PrintError("Yanlis secim");
                Pause();
                BookingFlow(user);
                return;
            }

            Department selectedDept = departments[deptIndex - 1];
            var doctors = _doctorService.GetApprovedByDepartment(selectedDept);

            if (doctors.Count == 0)
            {
                PrintError("Bu sobede hal-hazirda hekim yoxdu");
                Pause();
                BookingFlow(user);
                return;
            }

            int doctorBackOption = doctors.Count + 1;

            PrintHeader($"{selectedDept} Sobe Hekimleri");
            for (int i = 0; i < doctors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {doctors[i].TamAd}  (Is tecrubesi: {doctors[i].IsTecrubesi} il)");
            }
            Console.WriteLine($"{doctorBackOption}. Geri");
            Console.WriteLine();
            Console.Write("Hekim secin: ");

            string? doctorInput = Console.ReadLine();
            if (doctorInput == doctorBackOption.ToString())
            {
                BookingFlow(user);
                return;
            }

            if (!int.TryParse(doctorInput, out int docIndex) || docIndex < 1 || docIndex > doctors.Count)
            {
                PrintError("Yanlis secim");
                Pause();
                BookingFlow(user);
                return;
            }

            Doctor selectedDoctor = doctors[docIndex - 1];
            SelectDateFlow(user, selectedDoctor);
        }

        private static void SelectDateFlow(User user, Doctor doctor)
        {
            var dates = _appointmentService.GetAvailableDatesForDoctor(doctor.Id);
            int backOption = dates.Count + 1;

            PrintHeader($"{doctor.TamAd} Ucun tarix secin");
            for (int i = 0; i < dates.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dates[i]:dd.MM.yyyy dddd}");
            }
            Console.WriteLine($"{backOption}. Geri");
            Console.WriteLine();
            Console.Write("Seciminiz: ");
            string? input = Console.ReadLine();

            if (input == backOption.ToString())
            {
                BookingFlow(user);
                return;
            }

            if (!int.TryParse(input, out int dateIndex) || dateIndex < 1 || dateIndex > dates.Count)
            {
                PrintError("Yanlis secim");
                Pause();
                SelectDateFlow(user, doctor);
                return;
            }

            DateTime selectedDate = dates[dateIndex - 1];
            SelectSlotFlow(user, doctor, selectedDate);
        }

        private static void SelectSlotFlow(User user, Doctor doctor, DateTime date)
        {
            var slots = _appointmentService.GetSlotsForDoctorAndDate(doctor.Id, date);
            int backOption = slots.Count + 1;

            PrintHeader($"{doctor.TamAd} - {date:dd.MM.yyyy} Ucun Saatlar");
            for (int i = 0; i < slots.Count; i++)
            {
                string status = slots[i].IsReserved ? "Rezerv olub" : "Rezerv olmayib";
                Console.WriteLine($"{i + 1}. {slots[i].SaatAraligi}   ->   {status}");
            }
            Console.WriteLine($"{backOption}. Geri");
            Console.WriteLine();
            Console.Write("Saat secin: ");

            string? input = Console.ReadLine();
            if (input == backOption.ToString())
            {
                SelectDateFlow(user, doctor);
                return;
            }

            if (!int.TryParse(input, out int slotIndex) || slotIndex < 1 || slotIndex > slots.Count)
            {
                PrintError("Yanlis secim");
                Pause();
                SelectSlotFlow(user, doctor, date);
                return;
            }

            var selectedSlot = slots[slotIndex - 1];

            if (selectedSlot.IsReserved)
            {
                PrintError($"{selectedSlot.SaatAraligi} vaxt artiq rezerv olunub. Basqa vaxt secin");
                Pause();
                SelectSlotFlow(user, doctor, date);
                return;
            }

            _appointmentService.Reserve(selectedSlot.Id, user);

            PrintSuccess(
                $"Tesekkurler {user.TamAd}, siz {date:dd.MM.yyyy} tarixinde saat {selectedSlot.SaatAraligi} " +
                $"araliginda {doctor.TamAd} hekiminin qebuluna yazildiniz.");

            _notificationService.SendEmail(user.Email, "Qebul Rezervasiyasi Tesdiqi",
                $"Hormetli {user.TamAd}, siz {date:dd.MM.yyyy} tarixi, saat {selectedSlot.SaatAraligi} " +
                $"araliginda {doctor.TamAd} ({doctor.Sobe}) hekiminin qebuluna ugurla yazildiniz.");

            string receiptPath = _receiptService.GenerateReceipt(user, doctor, selectedSlot);
            Console.WriteLine($"Cekiniz yaradildi: {receiptPath}");

            Pause();
            MainMenu.ShowMainMenu();
        }
    }
}
