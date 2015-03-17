using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    abstract class DownloadJob : IJob
    {
        public abstract void Start();

        protected void DownloadAndInstall(string url)
        {
            Console.WriteLine("Download: " + url);
            var setup = Downloader.DownloadFile(url);
            if (setup != null && setup.Exists)
            {
                Console.WriteLine("Download successfull.");

                ProcessStartInfo startInfo = new ProcessStartInfo(setup.FullName)
                {
                    Arguments = @"/passive /norestart",
                    UseShellExecute = true,
                    Verb = "runas"
                };

                Process process = null;
                try
                {
                    process = Process.Start(startInfo);
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Error while installing (FileNotFound): " + setup.FullName);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine("Error while installing (Win32): " + setup.FullName);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }

                if (process != null)
                {
                    process.WaitForExit();
                    Console.WriteLine("Installation successfull.");
                    setup.Delete();
                }
                else
                {
                    Console.WriteLine("Installation failed.");
                }
            }
            else
            {
                Console.WriteLine("Download failed.");
            }
        }
    }
}
