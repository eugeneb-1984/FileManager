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
            Console.WriteLine("Это консольный файловый менеджер.");
            Console.WriteLine("для вывода инструкции наберите man");
            Console.WriteLine("Введите команду");
            var command = Console.ReadLine();
            Command.Parse(command);
        }
    }
}
