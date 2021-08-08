using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManagerApp
{
    class Command
    {
        public static void ShowDirContents(string DirName, int LinesPerPage)
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
        public static void DisplayPage(string[] DirContents, int page)
        {
            Console.WriteLine($"Страница {page + 1}:");
            var output = DirContents.Skip(Properties.Settings.Default.LinesPerPage * page).Take(Properties.Settings.Default.LinesPerPage);
            for (int i = 0; i < output.ToArray().Length; i++)
            {
                Console.WriteLine(output.ElementAt(i));
            }
        }
        public static void GetNextUserChoice(int totalPages, ref int page, ref bool isDone, ref bool isEdgePage)
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
                    if (page + 1 >= totalPages)
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
        public static void ShowManual() //ToDo
        {
            Console.WriteLine("Скоро здесь будет инструкция");
            Console.ReadKey();
        }

        public static void Copy() //ToDo
        {
            Console.WriteLine("Копирование в разработке");
            Console.ReadKey();
        }

        public static void Delete() //ToDo
        {
            Console.WriteLine("Удаление в разработке");
            Console.ReadKey();
        }

        public static void GetFileInfo() //ToDo
        {
            Console.WriteLine("Получение информации о файлах в разработке");
            Console.ReadKey();
        }

        public static void Parse(string Command)
        {
           int LinesPerPage = Properties.Settings.Default.LinesPerPage;

           string [] Args = Command.Split();
           switch (Args[0])
            {
                case "dir":
                    ShowDirContents(Args[1], LinesPerPage);
                    break;

                case "man":
                    ShowManual();
                    break;

                case "cp":
                    Copy();
                    break;

                case "rm":
                    Delete();
                    break;

                case "info":
                    GetFileInfo();
                    break;

                default:
                    Console.WriteLine("Команда не распознана");
                    Console.ReadKey();
                    break;
            }
            

        }
    }
}
