using Hospital.Info;
using static Hospital.Info.ChoiceMenuInfo;
using static Hospital.Info.ConsoleInfo;

namespace Hospital.Menus
{
    public class MainMenu
    {
        public static void ShowMainMenu()
        {
            string[] menu = { "Admin Girisi", "User Qeydiyyati", "User Login", "Hekim Qeydiyyati", "Hekim Login", "Cixis" };
            while (true)
            {

                ChoiceMenuInfo c = new();
                int secim = c.Choices(menu, "Xestaxana Qebul Sistemi");
                switch (secim)
                {
                    case 0:
                        AdminMenus.AdminLoginFlow();
                        break;
                    case 1:
                        UserMenu.UserRegisterFlow();
                        break;
                    case 2:
                        UserMenu.UserLoginFlow();
                        break;
                    case 3:
                        DoctorMenu.DoctorRegisterFlow();
                        break;
                    case 4:
                        DoctorMenu.DoctorLoginFlow();
                        break;
                    case 5:
                        return;
                }
            }
        }
    }
}
