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
            ShowDirContents(DirName);
        }
        static void ShowDirContents(string DirName)
        {
            var DirContents = Directory.GetFileSystemEntries(DirName, "*", SearchOption.TopDirectoryOnly);
            int LinesPerPage = 3;
            int Pages = DirContents.Length / LinesPerPage;
            if (DirContents.Length % LinesPerPage != 0) Pages += 1;
            for (int page = 0; page < Pages; page++)
            {
                Console.WriteLine($"Printing page #{page + 1}");
                var output = DirContents.Skip(LinesPerPage * page).Take(LinesPerPage);
                for (int i = 0; i < output.ToArray().Length; i++)
                {
                    Console.WriteLine(output.ElementAt(i));
                }
                Console.WriteLine("");
            }
            Console.ReadKey();
        }

        static void ShowByPage(string[] DirContents, int PageNum, int PageSize)
        {

        }
    }
}
