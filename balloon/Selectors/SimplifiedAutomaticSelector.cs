using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace balloon.Selectors
{
    public class SimplifiedAutomaticSelector : ISelector
    {
        private BalloonMachine _machine;

        public void Run()
        {
            for (var i = 1; i < 8; i++)
            {
                _machine = new BalloonMachine();
                _machine.SetBalloonQueue(Array.ConvertAll(
                    File.ReadAllLines(@"C:\code\priv\dojos\balloon\balloon\Inputs\luftballons" + i + ".txt"), int.Parse));
                while (!IsFinished())
                {
                    if (_machine.GetPackageContent() >= 20)
                    {
                        _machine.Pack();
                        _machine.Goal = 20;
                    }
                    else
                    {
                        int action = ProvideAction();
                        _machine.Take(action);
                    }
                }
                Console.Out.WriteLine("File: " + i);
                _machine.PrintStatistics();
            }
            Console.In.Read();
        }

        public int ProvideAction()
        {
            var goal = _machine.Goal - _machine.GetPackageContent();
            var accumulatedBoxesContent = _machine.GetBoxContents().Sum();
            var achievablePackages = new bool[_machine.NumberOfBoxes, accumulatedBoxesContent+1]; // rows: intended sum, cols: included boxes
            if (accumulatedBoxesContent < goal)
            {
                return (Array.IndexOf(_machine.GetBoxContents(),_machine.GetBoxContents().Where(a => a > 0).Min()));
            }
            for (var includedBoxes = 0; includedBoxes < _machine.NumberOfBoxes; includedBoxes++)
            for (var intendedPackageContent = 0; intendedPackageContent <= accumulatedBoxesContent; intendedPackageContent++)
            {
                var currentBoxConent = _machine.GetBoxContents()[includedBoxes];
                achievablePackages[includedBoxes, intendedPackageContent] = currentBoxConent == intendedPackageContent;
                    if(achievablePackages[includedBoxes, intendedPackageContent]) continue;
                if (includedBoxes > 0)
                {
                    achievablePackages[includedBoxes, intendedPackageContent] =
                        achievablePackages[includedBoxes - 1, intendedPackageContent] ||
                        (intendedPackageContent - currentBoxConent) >= 0 && achievablePackages[includedBoxes - 1,
                            intendedPackageContent - currentBoxConent];
                }
            }

            var bestPackageContent = accumulatedBoxesContent;
            for (var i = goal; i <= accumulatedBoxesContent; i++)
            {
                if (achievablePackages[_machine.NumberOfBoxes - 1, i])
                {
                    bestPackageContent = i;
                    break;
                }
            }

            var usedBoxes = new List<int>();
            for (var i = 9; i >= 0; i--)
            {
                var currentBoxContent = _machine.GetBoxContents()[i];
                if (currentBoxContent == bestPackageContent)
                {
                    usedBoxes.Add(i);
                    break;
                }

                if (i == 0 || !achievablePackages[i - 1, bestPackageContent])
                {
                    usedBoxes.Add(i);
                    bestPackageContent -= currentBoxContent;
                }
            }

            if (usedBoxes.Count == 0)
            {
                usedBoxes.Add(Array.IndexOf(_machine.GetBoxContents(), _machine.GetBoxContents().Where(i => i> 0).Min()));
            }
            return (Array.IndexOf(_machine.GetBoxContents(), usedBoxes.Select(i => _machine.GetBoxContents()[i]).Min()));
        }


        public bool IsFinished()
        {
            return _machine.HasEmptyBoxes() && _machine.GetPackageContent() < 20;
        }

        public void SetBalloonMachine(BalloonMachine balloonMachine)
        {
            _machine = balloonMachine;
        }
    }
}