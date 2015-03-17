using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    class Vcredist2010Check : RegistryCheckAndDownloadJob
    {
        public override void Start()
        {
            Console.WriteLine("Start checking for vcredist2010.");
            if (Environment.Is64BitOperatingSystem)
            {
                Check(@"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x64",
                    @"http://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe",
                   "vcredist2010 x64");
            }
            Check(@"SOFTWARE\Microsoft\VisualStudio\10.0\VC\VCRedist\x86",
                    @"http://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe",
                   "vcredist2010 x68");
            Console.WriteLine("Finshed checking for vcredist2010.");

        }
    }
}
