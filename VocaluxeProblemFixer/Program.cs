using System;
using System.Collections.Generic;
using VocaluxeProblemFixer.Jobs;

namespace VocaluxeProblemFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.WriteLogLine("-----------------------------------------------------\n");
            Log.WriteLogLine("Vocaluxe problem fixer by lukeIam\n");
            
            List<IJob> checkList = new List<IJob>
            {
                new Vcredist2010Check(),
                new Vcredist2012Check(),
                new DirectX9CCheck(),
                new GStreamerCheck(),
                new BlockedFilesCheck(),
                new SingStarMicCheck()
            };

            foreach (var job in checkList)
            {
                Log.WriteLogLine("-----------------------------------------------------\n");
                job.Start();
            }
            Log.WriteLogLine("-----------------------------------------------------\n");
            Log.WriteSuccessLine("All checks finished (you may need to restart your computer).");
            Console.ReadKey();
        }
    }
}
