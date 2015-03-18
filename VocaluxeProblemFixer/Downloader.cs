using System;
using System.IO;
using System.Net;

namespace VocaluxeProblemFixer
{
    public static class Downloader
    {
        private const string Folder = "VocaluxeFixer\\";

        public static FileInfo DownloadFile(string url)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Folder);
            Directory.CreateDirectory(tempPath);
            string filename = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            FileInfo file = new FileInfo(Path.Combine(tempPath, filename)); ;

            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(url, file.FullName);
                }
                catch (WebException e)
                {
                    Log.WriteErrorLine("Error while downloading file: " + url + " to " + file.FullName);
                    Log.WriteErrorLine(e.Message);
                    Log.WriteErrorLine(e.StackTrace);
                    return null;
                }
            }

            return file;
        }
    }
}
