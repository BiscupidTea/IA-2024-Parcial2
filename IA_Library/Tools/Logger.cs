using System;

namespace IA_Library.Tools
{
    public static class Logger
    {
        public static void LogInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[INFO]: {message}");
            Console.ResetColor(); 
        }
        
        public static void LogWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine($"[WARNING]: {message}");
            Console.ResetColor();
        }
        
        public static void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR]: {message}");
            Console.ResetColor();
        }
    }
}