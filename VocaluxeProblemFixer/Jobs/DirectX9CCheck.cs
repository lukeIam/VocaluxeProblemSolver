using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    class DirectX9CCheck : IJob
    {
        public void Start()
        {
            Console.WriteLine("Start checking for directX 9.0c.");
            Check();
            Console.WriteLine("Finshed checking for directX 9.0c.");

        }

        private void Check()
        {
            var folder = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                @"SOFTWARE\MICROSOFT\DIRECTX") ??
                         RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    @"SOFTWARE\MICROSOFT\DIRECTX");

            if (folder == null || folder.GetValue("Version").ToString() != "4.09.00.0904")
            {
                Console.WriteLine("directX 9.0c is missing.");
                DownloadAndInstall(
                   @"http://download.microsoft.com/download/8/0/d/80d7e79d-c0e4-415a-bcca-e229eafe2679/dxwebsetup.exe");


            }
            else
            {
                Console.WriteLine("Found directX 9.0c.");
            }
        }




        private void DownloadAndInstall(string url)
        {
            Console.WriteLine("Download: " + url);
            var setup = Downloader.DownloadFile(url);

            if (setup != null && setup.Exists)
            {
                Console.WriteLine("Download successfull.");

                ProcessStartInfo startInfo = new ProcessStartInfo(setup.FullName)
                {
                    Arguments = @"/Q",
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
