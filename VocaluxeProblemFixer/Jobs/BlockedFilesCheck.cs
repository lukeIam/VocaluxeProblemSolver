using System;
using System.IO;
using System.Linq;
using Trinet.Core.IO.Ntfs;

namespace VocaluxeProblemFixer.Jobs
{
    class BlockedFilesCheck : IJob
    {
        public void Start()
        {
            Log.WriteLogLine("Start checking the Vocaluxe folder for blocked files.");
            var folder = GetVocaluxeFolder();
            if (string.IsNullOrEmpty(folder))
            {
                Log.WriteErrorLine("Could not find the Vocaluxe folder\n(please place the VocalxeProblemSolver next to the Vocaluxe.exe).");
            }
            else
            {
                Log.WriteSuccessLine(String.Format("Found and fixed {0} files.", CheckAndUnblockDirectory(folder).ToString()));
            }
            Log.WriteLogLine("Finshed checking the Vocaluxe folder for blocked files.");
        }

        private string GetVocaluxeFolder()
        {
            var directory = Directory.GetCurrentDirectory();

            if (File.Exists(Path.Combine(directory, "Vocaluxe.exe")))
            {
                return directory;
            }
            directory = Path.Combine(directory, "Vocaluxe\\");
            if (File.Exists(Path.Combine(directory, "Vocaluxe.exe")))
            {
                return directory;
            }
            return null;
        }

        private int CheckAndUnblockDirectory(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] dirs = Directory.GetDirectories(path);

            return files.Count(CheckAndUnblockFile) + dirs.Sum(dir => CheckAndUnblockDirectory(dir));
        }

        private bool CheckAndUnblockFile(string fileName)
        {
            var file = new FileInfo(fileName);
            if (file.AlternateDataStreamExists("Zone.Identifier"))
            {
                return file.GetAlternateDataStream("Zone.Identifier",
                                            FileMode.Open).Delete();
            }

            return false;
        }




    }
}
