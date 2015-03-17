using Microsoft.Win32;
using System;


namespace VocaluxeProblemFixer.Jobs
{
    class Vcredist2012Check : RegistryCheckAndDownloadJob
    {
        public override void Start()
        {
            Console.WriteLine("Start checking for vcredist2012.");
            if (Environment.Is64BitOperatingSystem)
            {
                Check(@"SOFTWARE\Microsoft\VisualStudio\11.0\VC\Runtimes\x64",
                     @"http://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x64.exe",
                    "vcredist2012 x64");
            }
            Check(@"SOFTWARE\Microsoft\VisualStudio\11.0\VC\Runtimes\x86",
                    @"http://download.microsoft.com/download/1/6/B/16B06F60-3B20-4FF2-B699-5E9B7962F9AE/VSU_4/vcredist_x86.exe",
                   "vcredist2012 x86");
            Console.WriteLine("Finshed checking for vcredist2012.");

        }
    }
}
