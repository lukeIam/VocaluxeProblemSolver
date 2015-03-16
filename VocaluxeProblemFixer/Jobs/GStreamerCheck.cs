using System;
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

        private void DownloadAndInstall(string url)
        {
            Console.WriteLine("Download: " + url);
            var setup = Downloader.DownloadFile(url);
            Console.WriteLine("Download successfull.");
            //C:\Users\lucas_000\AppData\Local\Temp\VocaluxeFixer\gstreamer-1.0-x86_64-1.4.5.msi
            //setup.FullName
            ProcessStartInfo startInfo = new ProcessStartInfo("msiexec.exe")
            {
                Arguments = @"/package " + @"C:\Users\lucas_000\AppData\Local\Temp\VocaluxeFixer\gstreamer-1.0-x86_64-1.4.5.msi" + @" /passive /norestart ADDLOCAL=_gstreamer_1.0",
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
