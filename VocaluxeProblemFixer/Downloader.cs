using System;
using System.IO;
using System.Net;

namespace VocaluxeProblemFixer
{
    public static class Downloader
    {
        private static string _folder = "VocaluxeFixer\\";
        public static FileInfo DownloadFile(string url)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), _folder);
            string filename = url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1);
            FileInfo file = new FileInfo(Path.Combine(tempPath, filename)); ;
            
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(url, file.FullName);
            }

            return file;
        }
    }
}
