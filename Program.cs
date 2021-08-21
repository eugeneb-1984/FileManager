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

                    Console.WriteLine("Введите команду");
                    var command = Console.ReadLine();
                    if (command != "done")
                    {
                        Command.Parse(command);
                    }
                    else
                    {
                        isDone = true;
                    }
                }
                catch (FileNotFoundException ErrorFileNotFound)
                {
                    Console.WriteLine($"Ошибка: файл {ErrorFileNotFound.FileName} не найден.");
                }
                catch (IOException)
                {
                    Console.WriteLine($"Ошибка: файл уже существует в целевом каталоге. Если вы хотите перезаписать его, используйте ключ \"-o\"");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла неизвестная ошибка: \n {ex.GetType()} \n {ex.GetType()} \n {ex.Message}");
                }
            }
        }
    }
}
