using System;
using System.Linq;
using VocaluxeProblemFixer.Mic;

namespace VocaluxeProblemFixer.Jobs
{
    class SingStarMicCheck : IJob
    {
        public void Start()
        {
            Console.WriteLine("Start checking the configuration of your singstar microphones.");
            var result = Microphone.FixSingstarMicrophones();
            int fixedMicsCount = (from m in result
                                  where m.Value
                                  select m).Count();
            Console.WriteLine("Found {0} singstar microphone(s):\n{1} microphone(s) was/were configured well and {2} microphone(s) was/where fixed.", result.Count, result.Count - fixedMicsCount, fixedMicsCount);
            Console.WriteLine("Finshed checking the configuration of your singstar microphones.");
        }
    }
}
