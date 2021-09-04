using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManagerApp
{
    public class App
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
        public static void ShowManual()
        {
            Console.WriteLine(File.ReadAllText(Properties.Settings.Default.manPath));
        }

        public static void Copy(string SourcePath, string TargetPath, bool mustOverwrite)
        {
            //Если копируем каталог, добавляем его в TargetPath
            FileAttributes sourcePathAttr = File.GetAttributes(SourcePath);
            if (sourcePathAttr.HasFlag(FileAttributes.Directory))
            {
                FileInfo sourcePathInfo = new FileInfo(SourcePath);
                TargetPath = Path.Combine(TargetPath, sourcePathInfo.Name);
            }

            //Если нет каталога по TargetPath, создаём его
            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }

            //Копируем файлы каталога
            if (sourcePathAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo dirSource = new DirectoryInfo(SourcePath);
                FileInfo[] files = dirSource.GetFiles();
                foreach (FileInfo file in files)
                {
                    string tempPath = Path.Combine(TargetPath, file.Name);
                    file.CopyTo(tempPath, mustOverwrite);
                }
                // Отрабатываем подкаталоги
                DirectoryInfo[] subDirs = dirSource.GetDirectories();

                foreach (DirectoryInfo subdir in subDirs)
                {
                    string tempPath = Path.Combine(TargetPath, subdir.Name);
                    Copy(subdir.FullName, tempPath, mustOverwrite);
                }
            }
            else
            {
                FileInfo SourceFile = new FileInfo(SourcePath);
                TargetPath = Path.Combine(TargetPath, SourceFile.Name);
                SourceFile.CopyTo(TargetPath, mustOverwrite);
            }
        }

        public static void Move(string SourcePath, string TargetPath)
        {
            FileAttributes sourcePathAttr = File.GetAttributes(SourcePath);
            if (sourcePathAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo SourceDir = new DirectoryInfo(SourcePath);
                SourceDir.MoveTo(TargetPath);
            }
            else
            {
                FileInfo SourceFile = new FileInfo(SourcePath);
                SourceFile.MoveTo(TargetPath);
            }
        }

        public static void Delete(string Path) 
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

        public static void GetFileInfo(string Path)
        {
            long byteSize = new long(); 
            FileAttributes pathAttr = File.GetAttributes(Path);
            
            if (pathAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo pathInfo = new DirectoryInfo(Path);
                byteSize = GetDirSize(pathInfo);
            }
            else
            {
                FileInfo pathInfo = new FileInfo(Path);
                byteSize = pathInfo.Length;
            }

            Console.WriteLine($"\n");
            Console.WriteLine($"Информация о {Path}:");
            Console.WriteLine($"Это каталог: {pathAttr.HasFlag(FileAttributes.Directory)}");
            Console.WriteLine($"Системный: {pathAttr.HasFlag(FileAttributes.System)}");
            Console.WriteLine($"Только для чтения: {pathAttr.HasFlag(FileAttributes.ReadOnly)}");
            Console.WriteLine($"Размер в байтах: {byteSize}");
            Console.WriteLine($"\n");
        }

        public static void ChangeWorkDir(string Path)
        {
            if (!Directory.Exists(Path))
            {
                throw new Exception($"Каталог {Path} не существует");
            }
            DirectoryInfo newWorkDirInfo = new DirectoryInfo(Path);
            Properties.Settings.Default.workDir = newWorkDirInfo.FullName;
            Properties.Settings.Default.Save();
        }

        public static long GetDirSize (DirectoryInfo DirInfo)
        {
            long size = 0;
            
            FileInfo [] dirFilesInfo = DirInfo.GetFiles();
            foreach (FileInfo file in dirFilesInfo)
            {
                size += file.Length;
            }

            DirectoryInfo [] subDirsInfo = DirInfo.GetDirectories();
            foreach (DirectoryInfo subDir in subDirsInfo)
            {
                size += GetDirSize(subDir);
            }
            return size;
        }

        public static void CreateDir(string Path)
        {
            Directory.CreateDirectory(Path);
        }
    }
}
