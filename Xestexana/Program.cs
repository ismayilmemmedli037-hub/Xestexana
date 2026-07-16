using Hospital.Menus;
using static Hospital.Info.ConsoleInfo;

namespace Hospital
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            MainMenu.ShowMainMenu();
        }
    }
}