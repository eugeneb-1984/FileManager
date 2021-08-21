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
                    Console.WriteLine("Следующая: стрелка вправо | Предыдущая: стрелка влево | Выход: любая другая клавиша");
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

        public static void Copy(string SourcePath, string TargetPath, string OverwriteParam) //ToDo WorkDir
        {
            //Парсим OverwriteParam
            string overwriteParam = OverwriteParam;
            bool Overwrite = overwriteParam == "-o" ? true : false;

            // Создаём целевой каталог     
            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }

            FileAttributes attr = File.GetAttributes(SourcePath);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                // Получаем список файлов в исходном каталоге и копируем их в целевой каталог
                DirectoryInfo dirSource = new DirectoryInfo(SourcePath);
                FileInfo[] files = dirSource.GetFiles();
                foreach (FileInfo file in files)
                {
                    string tempPath = Path.Combine(TargetPath, file.Name);
                    file.CopyTo(tempPath, Overwrite);
                }
                // Отрабатываем подкаталоги
                DirectoryInfo[] subDirs = dirSource.GetDirectories();

                foreach (DirectoryInfo subdir in subDirs)
                {
                    string tempPath = Path.Combine(TargetPath, subdir.Name);
                    Copy(subdir.FullName, tempPath, overwriteParam);
                }
            }
            else
            {
                FileInfo SourceFile = new FileInfo(SourcePath);
                string tempPath = Path.Combine(TargetPath, SourceFile.Name);
                SourceFile.CopyTo(tempPath, Overwrite);
            }
        }

        public static void Move(string SourcePath, string TargetPath) //ToDo Exceptions and Overwriting
        {
            FileAttributes attr = File.GetAttributes(SourcePath);
            if (attr.HasFlag(FileAttributes.Directory))
            {
                Directory.Move(SourcePath, TargetPath);
            }
            else
            {
                FileInfo SourceFile = new FileInfo(SourcePath);
                SourceFile.MoveTo(TargetPath);
            }
        }

        public static void Delete(string Path) //ToDo Exceptions
        {
            FileAttributes attr = File.GetAttributes(Path);
            if (attr.HasFlag(FileAttributes.Directory)) 
            {
                //Удаляем файлы
                DirectoryInfo dirSource = new DirectoryInfo(Path);
                FileInfo[] files = dirSource.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }

                //Удаляем подкаталоги
                DirectoryInfo[] subDirs = dirSource.GetDirectories();
                foreach (DirectoryInfo subdir in subDirs)
                {
                    Delete(subdir.FullName);
                }

                //Удаляем каталог
                Directory.Delete(Path);
            }
            else
            {
                File.Delete(Path);
            }
        }

        public static void GetFileInfo() //ToDo
        {
            Console.WriteLine("Получение информации о файлах в разработке");
            Console.ReadKey();
        }

        public static void Parse(string Command)
        {
            int LinesPerPage = Properties.Settings.Default.LinesPerPage;

            string[] Args = new string[5];
            string[] Commands = Command.Split();
            for (int i = 0; i<Commands.Length; i++)
            {
                Args[i] = Commands[i];
            }

           switch (Args[0])
            {
                case "dir":
                    ShowDirContents(Args[1], LinesPerPage);
                    break;

                case "man":
                    ShowManual();
                    break;

                case "cp":
                    Copy(Args[1], Args[2], Args[3]);
                    break;

                case "mv":
                    Move(Args[1], Args[2]);
                    break;

                case "rm":
                    Delete(Args[1]);
                    break;

                case "info":
                    GetFileInfo();
                    break;

                default:
                    Console.WriteLine("Команда не распознана");
                    break;
            }
            

        }
    }
}
