using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    class GStreamerCheck : DownloadJob
    {
        public override void Start()
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
    }
}
