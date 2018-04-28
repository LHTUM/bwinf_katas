using System;
using System.IO;
using balloon.Selectors;

namespace balloon
{
    public class Program
    {
        private static BalloonMachine _balloonMachine;
        private static SimplifiedAutomaticSelector _selector;

        private static void Main()
        {
            _balloonMachine = new BalloonMachine();
            _selector = new SimplifiedAutomaticSelector();

            for (var i = 1; i < 8; i++)
            {
                var inputSequence = ReadSequence(i);
                ExecuteSequence(inputSequence, _selector);
                PrintExecutionStatistics(i);
            }
            Console.In.Read();
        }


        private static int[] ReadSequence(int i)
        {
            return ReadSequenceFromFile(@"C:\code\priv\dojos\balloon\balloon\Inputs\luftballons" + i + ".txt");
        }

        private static void ExecuteSequence(int[] inputSequence, ISelector selector)
        {
            _balloonMachine.SetBalloonQueue(inputSequence);
            selector.SetBalloonMachine(_balloonMachine);
            while (!selector.IsFinished())
            {
                selector.ExecuteStep();
            }
        }

        private static void PrintExecutionStatistics(int i)
        {
            Console.Out.WriteLine("File - " + i + " :");
            _balloonMachine.PrintStatistics();
        }

        private static int[] ReadSequenceFromFile(string uri)
        {
            return Array.ConvertAll(
                File.ReadAllLines(uri),
                int.Parse);
        }
    }
}
