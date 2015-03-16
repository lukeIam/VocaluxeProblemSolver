using Microsoft.Win32;
using System;
using System.Diagnostics;


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
            Console.WriteLine("Download successfull.");

            ProcessStartInfo startInfo = new ProcessStartInfo(setup.FullName)
            {
                Arguments = @"/Q",
                UseShellExecute = true,
                Verb = "runas"
            };

            var process = Process.Start(startInfo);

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
    }
}
