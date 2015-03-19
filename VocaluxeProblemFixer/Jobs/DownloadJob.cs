using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;


namespace VocaluxeProblemFixer.Jobs
{
    abstract class DownloadJob : IJob
    {
        public abstract void Start();

        protected void DownloadAndInstall(string url, string runParameter)
        {
            Log.WriteLogLine("Download: " + url);
            var setup = Downloader.DownloadFile(url);
            if (setup != null && setup.Exists)
            {
                Log.WriteSuccessLine("Download successfull.");

                ProcessStartInfo startInfo = new ProcessStartInfo(setup.FullName)
                {
                    Arguments = runParameter,
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
                    Log.WriteErrorLine("Error while installing (FileNotFound): " + setup.FullName);
                    Log.WriteErrorLine(e.Message);
                    Log.WriteErrorLine(e.StackTrace);
                }
                catch (Win32Exception e)
                {
                    Log.WriteErrorLine("Error while installing (Win32): " + setup.FullName);
                    Log.WriteErrorLine(e.Message);
                    Log.WriteErrorLine(e.StackTrace);
                }

                if (process != null)
                {
                    process.WaitForExit();
                    Log.WriteSuccessLine("Installation successfull.");

                    try
                    {
                        setup.Delete();
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Log.WriteLogLine("Error while deleting setup file.");
                        Log.WriteLogLine(e.Message);
                        Log.WriteLogLine(e.StackTrace);
                    }
                    catch (IOException e)
                    {
                        Log.WriteLogLine("Error while deleting setup file.");
                        Log.WriteLogLine(e.Message);
                        Log.WriteLogLine(e.StackTrace);
                    }
                    catch (UnauthorizedAccessException e)
                    {
                        Log.WriteLogLine("Error while deleting setup file.");
                        Log.WriteLogLine(e.Message);
                        Log.WriteLogLine(e.StackTrace);
                    }
                    catch (ArgumentException e)
                    {
                        Log.WriteLogLine("Error while deleting setup file.");
                        Log.WriteLogLine(e.Message);
                        Log.WriteLogLine(e.StackTrace);
                    }
                }
                else
                {
                    Log.WriteErrorLine("Installation failed.");
                }
            }
            else
            {
                Log.WriteErrorLine("Download failed.");
            }
        }
    }
}
