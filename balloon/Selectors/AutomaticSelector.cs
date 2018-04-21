using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace balloon.Selectors
{
    public class AutomaticSelector : ISelector
    {
        private BalloonMachine _machine;

        public void Run()
        {
            for (int i = 1; i < 8; i++)
            {
                _machine = new BalloonMachine();
                _machine.SetBalloonQueue(Array.ConvertAll(
                    File.ReadAllLines(@"C:\code\priv\dojos\balloon\balloon\Inputs\luftballons"+ i +".txt"), int.Parse));
                while (!IsFinished())
                {
                    if (_machine.GetCurrentPackageContent() >= 20)
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
            bool[,] possible = new bool[10,
                _machine.GetCurrentBoxContents().Aggregate((a, b) => a >= _machine.Goal ? a : a + b) + 1];
            if (possible.GetLength(1) - _machine.Goal <= 0)
            {
                //Falls mit den aktuellen Mengen das Ziel nicht erreicht wird
                //Fach mit dem kleinsten Inhalt auskippen
                _machine.Goal -= _machine.GetCurrentBoxContents().Min();
                return (Array.IndexOf(_machine.GetCurrentBoxContents(), _machine.GetCurrentBoxContents().Where(a=> a>0).Min()));
            }
            else
            {
                //Berechnen des Hauptproblems
                for (int i = 0; i < 10; i++)
                for (int j = 0; j < possible.GetLength(1); j++)
                    possible[i, j] = _machine.GetCurrentBoxContents()[i] == j || i > 0 &&
                                     (possible[i - 1, j] || j - _machine.GetCurrentBoxContents()[i] >= 0 &&
                                      possible[i - 1, j - _machine.GetCurrentBoxContents()[i]]);
                //Prüfen, was die bestmögliche Anzahl ist:
                int found = Enumerable.Range(_machine.Goal, possible.GetLength(1) - _machine.Goal)
                    .FirstOrDefault(a => possible[9, a]);
                //Benutzte Fächer rekonstruieren
                List<int> used = new List<int>();
                for (int i = 9; i >= 0; i--)
                {
                    if (_machine.GetCurrentBoxContents()[i] == found)
                    {
                        used.Add(i);
                        break;
                    }

                    if (found - _machine.GetCurrentBoxContents()[i] >= 0 &&
                        possible[i - 1, found - _machine.GetCurrentBoxContents()[i]] &&
                        (i == 0 || !possible[i - 1, found]))
                    {
                        used.Add(i);
                        found -= _machine.GetCurrentBoxContents()[i];
                    }
                }

                //Fach mit dem kleinsten Inhalt auskippen
                _machine.Goal -= used.Select(i => _machine.GetCurrentBoxContents()[i]).Min();
                return (Array.IndexOf(_machine.GetCurrentBoxContents(),
                    used.Select(i => _machine.GetCurrentBoxContents()[i]).Min()));
            }
        }


        public bool IsFinished()
        {
            if (_machine.HasEmptyBoxes() && _machine.GetCurrentPackageContent() < 20)
            {
                return true;
            }

            return false;
        }

        public void SetBalloonMachine(BalloonMachine balloonMachine)
        {
            _machine = balloonMachine;
        }
    }
}