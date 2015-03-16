using System;
using System.Collections.Generic;
using VocaluxeProblemFixer.Jobs;

namespace VocaluxeProblemFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------------------------------------\n");
            Console.WriteLine("Vocaluxe problem fixer by lukeIam\n");
            
            List<IJob> checkList = new List<IJob>
            {
                new Vcredist2010Check(),
                new Vcredist2012Check(),
                new DirectX9CCheck(),
                new GStreamerCheck(),
                new SingStarMicCheck()
            };

            foreach (var job in checkList)
            {
                Console.WriteLine("-----------------------------------------------------\n");
                job.Start();
            }
            Console.WriteLine("-----------------------------------------------------\n");
            Console.WriteLine("All checks finished (you may need to restart your computer).");
            Console.ReadKey();
        }
    }
}
