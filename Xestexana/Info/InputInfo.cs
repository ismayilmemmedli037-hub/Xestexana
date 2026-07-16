using static Hospital.Info.ConsoleInfo;
namespace Hospital.Info
{
    public class InputInfo
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                PrintError("Zehmet olmasa reqem daxil et");
            }
        }
    }
}
