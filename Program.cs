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
            Console.WriteLine($"Вывод разбит на {Pages} страниц.\n");
            bool isDone = false;
            bool isEdgePage = false;
            int page = 0;
            while (!isDone)
            {
                if (!isEdgePage)
                {
                    DisplayPage(DirContents, page);
                    Console.WriteLine("Следующая: стрелка вправо | Предыдущая: стрелка влево | Возврат: любая другая клавиша");
                }
                GetNextUserChoice(Pages, ref page, ref isDone, ref isEdgePage);
            }

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
        static void GetNextUserChoice (int totalPages, ref int page, ref bool isDone, ref bool isEdgePage)
        {
            var input = Console.ReadKey();
            switch (input.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (page - 1 < 0)
                    {
                        Console.WriteLine("Вы уже находитесь на самой первой странице");
                        isEdgePage = true;
                    }
                    else
                    {
                        page--;
                        isEdgePage = false;
                    }
                    break;

                case ConsoleKey.RightArrow:
                    if (page + 2 > totalPages)
                    {
                        Console.WriteLine("Вы уже находитесь на самой последней странице");
                        isEdgePage = true;
                    }
                    else
                    {
                        page++;
                        isEdgePage = false;
                    }
                    break;

                default:
                    Console.WriteLine("Ок, выходим");
                    isDone = true;
                    break;
            }
        }
    }
}
