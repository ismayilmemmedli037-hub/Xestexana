using Hospital.Back;
using static Hospital.Info.ConsoleInfo;
using Hospital.Info;
using Hospital.Properties;
namespace Hospital.Menus
{
    public class AdminMenus
    {
        private const string AdminUsername = "Ismayil";
        private const string AdminPassword = "2010";
        public static void AdminLoginFlow()
        {
            PrintHeader("Admin Girisi");
            Console.Write("Istifadeci adi: ");
            string username = Console.ReadLine() ?? "";
            Console.Write("Sifre: ");
            string password = Console.ReadLine() ?? "";
            if (username != AdminUsername || password != AdminPassword)
            {
                PrintError("Istifadeci adi ve ya sifre yanlisdi");
                Pause();
                return;
            }

            AdminMenu();
        }

        private static void AdminMenu()
        {
            string[] menu = {"Gozleyen hekim muraciatlerine bax", "Butun hekimlere bax", "Butun istifadecilere bax", "Butun rezervasiyalara bax", "Cixis" };

            while (true)
            {
                ChoiceMenuInfo c = new ChoiceMenuInfo();
                int secim = c.Choices(menu, "Admin Paneli");

                switch (secim)
                {
                    case 0:
                        ReviewPendingDoctors();
                        break;
                    case 1:
                        ListAllDoctors();
                        break;
                    case 2:
                        ListAllUsers();
                        break;
                    case 3:
                        ListAllReservations();
                        break;
                    case 4:
                        return;
                }
            }
        }

        private static void ReviewPendingDoctors()
        {
            var pending = AppServices.DoctorService.GetPending();
            if (pending.Count == 0)
            {
                PrintError("Hal-hazirda gozleyen muraciat yoxdu");
                Pause();
                return;
            }

            foreach (var doctor in pending)
            {
                PrintHeader("Hekim Muracieti");
                Console.WriteLine($"Ad Soyad: {doctor.TamAd}");
                Console.WriteLine($"Sobe: {doctor.Sobe}");
                Console.WriteLine($"Tecrube: {doctor.IsTecrubesi} il");
                Console.WriteLine($"Email: {doctor.Email}");
                Console.WriteLine();
                Console.WriteLine("1. Qebul et");
                Console.WriteLine("2. Legv et");
                Console.WriteLine("3. Bu muraciati kec");
                Console.WriteLine();
                Console.Write("Seciminiz: ");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AppServices.DoctorService.Approve(doctor.Id);
                        PrintSuccess($"{doctor.TamAd} ise qebul olundu ve {doctor.Sobe} sobesinde kecin");
                        break;
                    case "2":
                        AppServices.DoctorService.Reject(doctor.Id);
                        PrintError($"{doctor.TamAd} muraciet redd edildi");
                        break;
                    default:
                        Console.WriteLine("Kecildi");
                        break;
                }
                Pause();
            }
        }

private static void ListAllDoctors()
{
    PrintHeader("Butun Hekimler");
    var doctors = AppServices.DoctorService.GetAll()
        .Where(d => d.Status == DoctorStatus.Approved)
        .ToList();

    if (doctors.Count == 0)
    {
        PrintError("Hec bir tesdiqlenmis hekim yoxdu");
    }

    foreach (var doctor in doctors)
    {
        Console.WriteLine(
            $"{doctor.TamAd}  |  {doctor.Sobe}  |  " +
            $"{doctor.IsTecrubesi} il tecrube  |  {doctor.Email}");
    }
    Pause();
}

        private static void ListAllUsers()
        {
            PrintHeader("Butun Istifadeciler");
            foreach (var user in AppServices.UserService.GetAll())
            {
                Console.WriteLine($"{user.TamAd}  |  {user.Email}  |  {user.Telefon}");
            }
            Pause();
        }

        private static void ListAllReservations()
        {
            PrintHeader("Butun Rezervasiyalar");
            foreach (var slot in AppServices.AppointmentService.GetAllReserved().OrderBy(s => s.Tarix))
            {
                var doctor = AppServices.DoctorService.GetById(slot.DoctorId);
                Console.WriteLine($"{slot.Tarix:dd.MM.yyyy} {slot.SaatAraligi}  |  Hekim: {doctor?.TamAd}  |  Pasiyent: {slot.UserTamAd}");
            }
            Pause();
        }
    }
}
