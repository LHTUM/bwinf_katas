using System;
using System.IO;
using balloon.Selectors;

namespace balloon
{
    public class Program
    {
        static void Main()
        {
            var balloonMachine = new BalloonMachine();
            var selector = new SimplifiedAutomaticSelector();
            // var selector = new ManualSelector();
            // var selector = new AutomaticSelector();

            for (var i = 1; i < 8; i++)
            {
                var inputSequence =
                    ReadSequenceFromFile(@"C:\code\priv\dojos\balloon\balloon\Inputs\luftballons" + i + ".txt");

                balloonMachine.SetBalloonQueue(inputSequence);
                selector.SetBalloonMachine(balloonMachine);

                Console.Out.WriteLine("File - " + i + " :");
                selector.Run();
                balloonMachine.PrintStatistics();
            }
            Console.In.Read();
        }

        private static int[] ReadSequenceFromFile(string uri)
        {
            return Array.ConvertAll(
                File.ReadAllLines(uri),
                int.Parse);
        }
    }
}
