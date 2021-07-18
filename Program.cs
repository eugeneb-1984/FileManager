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
            Console.WriteLine("Введите путь к каталогу");
            string DirName = Console.ReadLine();
            ShowDirContents(DirName, Properties.Settings.Default.LinesPerPage);
        }
        static void ShowDirContents(string DirName, int LinesPerPage)
        {
            var DirContents = Directory.GetFileSystemEntries(DirName, "*", SearchOption.TopDirectoryOnly);
            int Pages = DirContents.Length / LinesPerPage;
            if (DirContents.Length % LinesPerPage != 0) Pages += 1;
            int page = 0;
            Console.WriteLine($"Вывод разбит на {Pages} страниц.\n");
            DisplayPage(DirContents, page);
        }
        static void DisplayPage(string[] DirContents, int page)
        {
            Console.WriteLine($"Страница {page + 1}:");
            var output = DirContents.Skip(Properties.Settings.Default.LinesPerPage * page).Take(Properties.Settings.Default.LinesPerPage);
            for (int i = 0; i < output.ToArray().Length; i++)
            {
                Console.WriteLine(output.ElementAt(i));
            }
        }
        static int GetNextUserChoice()
        {
            Console.WriteLine("Следующая: стрелка вправо | Предыдущая: стрелка влево | Возврат: любая другая клавиша");
            var input = Console.ReadKey();
            int UserChoice;
            switch (input.Key)
            {
                case ConsoleKey.LeftArrow:
                    UserChoice = 1;
                    break;

                case ConsoleKey.RightArrow:
                    UserChoice = 2;
                    break;

                default:
                    UserChoice = 3;
                    break;
            }
            return UserChoice;
        }
    }
}
