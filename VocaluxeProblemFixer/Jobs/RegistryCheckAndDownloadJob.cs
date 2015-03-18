using Microsoft.Win32;


namespace VocaluxeProblemFixer.Jobs
{
    abstract class RegistryCheckAndDownloadJob : DownloadJob
    {
        protected void Check(string key, string url, string name, string parameter)
        {
            if (
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(
                    key) == null &&
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(
                    key) == null)
            {
                Log.WriteLogLine(name + " is missing.");
                DownloadAndInstall(url, parameter);
            }
            else
            {
                Log.WriteSuccessLine("Found " + name + ".");
            }
        }
    }
}
