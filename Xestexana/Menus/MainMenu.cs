using static Hospital.Info.ConsoleInfo;
namespace Hospital.Menus
{
    public class MainMenu
    {
        public static void ShowMainMenu()
        {
            while (true)
            {
                PrintHeader("Xestaxana Qebul Sistemi");
                Console.WriteLine("1. Admin Girisi");
                Console.WriteLine("2. User Qeydiyyati");
                Console.WriteLine("3. User Login");
                Console.WriteLine("4. Hekim Qeydiyyati");
                Console.WriteLine("5. Hekim Login");
                Console.WriteLine("6. Cixis");
                Console.WriteLine();
                Console.Write("Seciminiz: ");
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        AdminMenus.AdminLoginFlow();
                        break;
                    case "2":
                        UserMenu.UserRegisterFlow();
                        break;
                    case "3":
                        UserMenu.UserLoginFlow();
                        break;
                    case "4":
                        DoctorMenu.DoctorRegisterFlow();
                        break;
                    case "5":
                        DoctorMenu.DoctorLoginFlow();
                        break;
                    case "6":
                        return;
                    default:
                        PrintError("Yanlis secim yeniden cehd edin");
                        break;
                }
            }
        }
    }
}
