using Microsoft.Win32;
using System;


namespace VocaluxeProblemFixer.Jobs
{
    class DirectX9CCheck : DownloadJob
    {
        public override void Start()
        {
            Log.WriteLogLine("Start checking for directX 9.0c.");
            Check();
            Log.WriteLogLine("Finshed checking for directX 9.0c.");

        }

        private void Check()
        {
            var folder = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                @"SOFTWARE\MICROSOFT\DIRECTX") ??
                         RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    @"SOFTWARE\MICROSOFT\DIRECTX");

            if (folder == null || folder.GetValue("Version").ToString() != "4.09.00.0904")
            {
                Log.WriteLogLine("directX 9.0c is missing.");
                DownloadAndInstall(
                   @"http://download.microsoft.com/download/8/0/d/80d7e79d-c0e4-415a-bcca-e229eafe2679/dxwebsetup.exe", @"/Q");


            }
            else
            {
                Log.WriteSuccessLine("Found directX 9.0c.");
            }
        }
    }
}
