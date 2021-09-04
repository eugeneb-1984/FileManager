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

            bool mustOverwrite = AppArgs.Any("-o".Equals);

            if (AppArgs[1] == null)
            {
                AppArgs[1] = "";
            }
            if (AppArgs[2] == null)
            {
                AppArgs[2] = "";
            }

            string path = ConstructPath(AppArgs[1]);
            string targetPath = ConstructPath(AppArgs[2]); 

            switch (AppArgs[0])
            {
                case "dir":
                    App.ShowDirContents(path, LinesPerPage);
                    break;

                case "man":
                    App.ShowManual();
                    break;

                case "cp":
                    App.Copy(path, targetPath, mustOverwrite);
                    break;

                case "mv":
                    App.Move(path, targetPath);
                    break;

                case "rm":
                    App.Delete(path);
                    break;

                case "info":
                    App.GetFileInfo(path);
                    break;

                case "cd":
                    App.ChangeWorkDir(path);
                    break;

                case "mkdir":
                    App.CreateDir(path);
                    break;

                default:
                    Console.WriteLine("Команда не распознана");
                    break;
            }
        }

        public static string ConstructPath(string sourcePath)
        {
            string workDir = Properties.Settings.Default.workDir;
            string resultPath = Path.Combine(workDir, sourcePath);
            return resultPath;
        }
    }
}
