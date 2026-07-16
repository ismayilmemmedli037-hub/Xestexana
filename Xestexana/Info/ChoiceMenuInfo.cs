using static Hospital.Info.ConsoleInfo;
namespace Hospital.Info
{
    public class ChoiceMenuInfo
    {
        public int Choices(string[] menu)
        {
            int selectedIndex = 0;
            ConsoleKey keyPressed;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();

                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {menu[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menu[i]}");
                    }
                }

                keyPressed = Console.ReadKey(true).Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = menu.Length - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex > menu.Length - 1)
                        selectedIndex = 0;
                }

            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }

        public int Choices(string[] menu, string text)
        {
            int selectedIndex = 0;
            ConsoleKey keyPressed;

            Console.CursorVisible = false;

            do
            {
                Console.Clear();
                PrintHeader(text);

                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {menu[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menu[i]}");
                    }
                }

                keyPressed = Console.ReadKey(true).Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = menu.Length - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex > menu.Length - 1)
                        selectedIndex = 0;
                }

            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }
    }
}
