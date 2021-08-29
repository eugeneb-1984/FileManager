using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace FileManagerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isDone = false;
            Console.WriteLine("Это консольный файловый менеджер.");
            Console.WriteLine("для вывода инструкции наберите man");
            while (!isDone)
            {
                try
                {
                    string currentDir = Properties.Settings.Default.workDir;
                    Console.Write($"{currentDir}>");
                    var command = Console.ReadLine();
                    if (command != "done")
                    {
                        Parser.Parse(command);
                    }
                    else
                    {
                        isDone = true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}
