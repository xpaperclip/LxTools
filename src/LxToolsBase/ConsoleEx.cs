using System;

namespace LxTools
{
    public static class ConsoleEx
    {
        public static void Write(ConsoleColor foregroundColor, string value)
        {
            Console.ForegroundColor = foregroundColor;
            Console.Write(value);
            Console.ResetColor();
        }
        public static void Write(ConsoleColor foregroundColor, string value, params object[] args)
        {
            Console.ForegroundColor = foregroundColor;
            Console.Write(value, args);
            Console.ResetColor();
        }
        public static void Write(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(value);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor foregroundColor, string value)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }
        public static void WriteLine(ConsoleColor foregroundColor, string value, params object[] args)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine(value, args);
            Console.ResetColor();
        }
        public static void WriteLine(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string value)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}
