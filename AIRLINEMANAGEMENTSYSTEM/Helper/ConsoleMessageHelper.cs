using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Helper
{
    public class ConsoleMessageHelper
    {
        public static void WriteError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void WriteTableHeader(string header)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(header);
            Console.ResetColor();
        }

        public static void WriteTableRow(string row)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(row);
            Console.ResetColor();
        }

        public static void WriteCommoncolor(string row)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(row);
            Console.ResetColor();
        }

        public static void WriteWithBackground(
       string message,
       ConsoleColor background,
       ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            Console.ForegroundColor = foreground;
            Console.WriteLine(message);
            Console.ResetColor();
        }


    }
}
