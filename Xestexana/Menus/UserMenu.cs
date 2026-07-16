using Hospital.Back;
using Hospital.Info;
using Hospital.Properties;
using static Hospital.Info.ConsoleInfo;

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
            ChoiceMenuInfo c = new();
            string[] deptMenu = new string[departments.Length + 1];
            for (int i = 0; i < departments.Length; i++)
            {
                deptMenu[i] = departments[i].ToString();
            }
            deptMenu[departments.Length] = "Geri";

            int deptSecim = c.Choices(deptMenu, "Sobe Secimi");

            if (deptSecim == departments.Length)
            {
                return;
            }

            Department selectedDept = departments[deptSecim];
            var doctors = _doctorService.GetApprovedByDepartment(selectedDept);

            if (doctors.Count == 0)
            {
                PrintError("Bu sobede hal-hazirda hekim yoxdu");
                Pause();
                BookingFlow(user);
                return;
            }

            string[] doctorMenu = new string[doctors.Count + 1];
            for (int i = 0; i < doctors.Count; i++)
            {
                doctorMenu[i] = $"{doctors[i].TamAd}  (Is tecrubesi: {doctors[i].IsTecrubesi} il)";
            }
            doctorMenu[doctors.Count] = "Geri";

            int docSecim = c.Choices(doctorMenu, $"{selectedDept} Sobe Hekimleri");
            if (docSecim == doctors.Count)
            {
                BookingFlow(user);
                return;
            }

            Doctor selectedDoctor = doctors[docSecim];
            SelectDateFlow(user, selectedDoctor);
        }

        private static void SelectDateFlow(User user, Doctor doctor)
        {
            var dates = _appointmentService.GetAvailableDatesForDoctor(doctor.Id);
            ChoiceMenuInfo c = new();
            string[] dateMenu = new string[dates.Count + 1];
            for (int i = 0; i < dates.Count; i++)
            {
                dateMenu[i] = $"{dates[i]:dd.MM.yyyy dddd}";
            }
            dateMenu[dates.Count] = "Geri";

            int secim = c.Choices(dateMenu, $"{doctor.TamAd} Ucun tarix secin");

            if (secim == dates.Count)
            {
                BookingFlow(user);
                return;
            }

            DateTime selectedDate = dates[secim];
            SelectSlotFlow(user, doctor, selectedDate);
        }

        private static void SelectSlotFlow(User user, Doctor doctor, DateTime date)
        {
            var slots = _appointmentService.GetSlotsForDoctorAndDate(doctor.Id, date);
            ChoiceMenuInfo c = new();

            string[] menu = new string[slots.Count + 1];
            for (int i = 0; i < slots.Count; i++)
            {
                string status = slots[i].IsReserved ? "Rezerv olub" : "Rezerv olmayib";
                menu[i] = $"{slots[i].SaatAraligi}   ->   {status}";
            }
            menu[slots.Count] = "Geri";

            int secim = c.Choices(menu, $"{doctor.TamAd} - {date:dd.MM.yyyy} Ucun Saatlar");

            if (secim == slots.Count)
            {
                SelectDateFlow(user, doctor);
                return;
            }

            var selectedSlot = slots[secim];

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
