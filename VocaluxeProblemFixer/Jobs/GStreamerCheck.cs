using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    class GStreamerCheck : IJob
    {
        public void Start()
        {
            Console.WriteLine("Start checking for gstreamer.");
            if (Environment.Is64BitOperatingSystem)
            {
                Checkx64();
            }
            else
            {
                Checkx86();
            }
            Console.WriteLine("Finshed checking for gstreamer.");

        }

        private void Checkx86()
        {
            string path = Environment.GetEnvironmentVariable("GSTREAMER_1_0_ROOT_X86");
            if (String.IsNullOrEmpty(path) || !Directory.Exists(path) || !File.Exists(path + @"lib\gstreamer-1.0\libgstmad.dll"))
            {
                Console.WriteLine("gstreamer x68 is missing or misconfigured.");
                DownloadAndInstall(
                   @"http://gstreamer.freedesktop.org/data/pkg/windows/1.4.5/gstreamer-1.0-x86-1.4.5.msi");
            }
            else
            {
                Console.WriteLine("Found gstreamer x68.");
            }
        }

        private void Checkx64()
        {
            string path = Environment.GetEnvironmentVariable("GSTREAMER_1_0_ROOT_X86_64");
            if (String.IsNullOrEmpty(path) || !Directory.Exists(path) || !File.Exists(path + @"lib\gstreamer-1.0\libgstmad.dll"))
            {
                Console.WriteLine("gstreamer x64 is missing or misconfigured.");
                DownloadAndInstall(
                   @"http://gstreamer.freedesktop.org/data/pkg/windows/1.4.5/gstreamer-1.0-x86_64-1.4.5.msi");
            }
            else
            {
                Console.WriteLine("Found gstreamer x64.");
            }
        }

        protected void DownloadAndInstall(string url)
        {
            Console.WriteLine("Download: " + url);
            var setup = Downloader.DownloadFile(url);
            if (setup != null && setup.Exists)
            {
                Console.WriteLine("Download successfull.");

                ProcessStartInfo startInfo = new ProcessStartInfo("msiexec.exe")
                {
                    Arguments = @"/package " + setup.FullName + @" /passive /norestart ADDLOCAL=_gstreamer_1.0",       
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
