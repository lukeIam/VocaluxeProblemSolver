using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    class Vcredist2010Check : IJob
    {
        public void Start()
        {
            Console.WriteLine("Start checking for vcredist2010.");
            if (Environment.Is64BitOperatingSystem)
            {
                Checkx64();
            }
            Checkx86();
            Console.WriteLine("Finshed checking for vcredist2010.");

        }

        private void Checkx86()
        {
            if (
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                    @"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86") == null &&
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    @"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86") == null)
            {
                Console.WriteLine("vcredist2010 x68 is missing.");
                DownloadAndInstall(
                   @"http://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe");


            }
            else
            {
                Console.WriteLine("Found vcredist2010 x68.");
            }
        }

        private void Checkx64()
        {
            if (
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                    @"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x64") == null &&
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    @"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x64") == null)
            {
                Console.WriteLine("vcredist2010 x64 is missing.");
                DownloadAndInstall(
                   @"http://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe");

            }
            else
            {
                Console.WriteLine("Found vcredist2010 x64.");
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
