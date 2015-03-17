using Microsoft.Win32;
using System;


namespace VocaluxeProblemFixer.Jobs
{
    abstract class RegistryCheckAndDownloadJob : DownloadJob
    {
        protected void Check(string key, string url, string name)
        {
            if (
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                    key) == null &&
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    key) == null)
            {
                Console.WriteLine(name + " is missing.");
                DownloadAndInstall(url);
            }
            else
            {
                Console.WriteLine("Found " + name + ".");
            }
        }
    }
}
