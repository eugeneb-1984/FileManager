using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileManagerApp
{
    public class Parser
    {
        public static void Parse(string Command)
        {
            int LinesPerPage = Properties.Settings.Default.LinesPerPage;

            string[] AppArgs = new string[5];
            string[] CommandArgs = Command.Split();
            for (int i = 0; i < CommandArgs.Length; i++)
            {
                AppArgs[i] = CommandArgs[i];
            }

            bool isAbsoluteSource = AppArgs.Any("-as".Equals);
            bool isAbsoluteTarget = AppArgs.Any("-at".Equals);
            bool mustOverwrite = AppArgs.Any("-o".Equals);

            if (AppArgs[1] == null)
            {
                AppArgs[1] = "";
            }
            if (AppArgs[2] == null)
            {
                AppArgs[2] = "";
            }

            string sourcePath = ConstructPath(AppArgs[1], isAbsoluteSource);
            string targetPath = ConstructPath(AppArgs[2], isAbsoluteTarget); 

            switch (AppArgs[0])
            {
                case "dir":
                    App.ShowDirContents(sourcePath, LinesPerPage);
                    break;

                case "man":
                    App.ShowManual();
                    break;

                case "cp":
                    App.Copy(sourcePath, targetPath, mustOverwrite);
                    break;

                case "mv":
                    App.Move(sourcePath, targetPath);
                    break;

                case "rm":
                    App.Delete(sourcePath);
                    break;

                case "info":
                    App.GetFileInfo(sourcePath);
                    break;

                case "cd":
                    App.ChangeWorkDir(sourcePath);
                    break;

                default:
                    Console.WriteLine("Команда не распознана");
                    break;
            }
        }

        public static string ConstructPath(string sourcePath, bool isAbsolute)
        {
            string workDir = Properties.Settings.Default.workDir;
            string resultPath = isAbsolute ? sourcePath : Path.Combine(workDir, sourcePath);
            return resultPath;
        }
    }
}
