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
            for (var i = 1; i < 8; i++)
            {
                _machine = new BalloonMachine();
                _machine.SetBalloonQueue(Array.ConvertAll(
                    File.ReadAllLines(@"C:\code\priv\dojos\balloon\balloon\Inputs\luftballons"+ i +".txt"), int.Parse));
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
            var possible = new bool[10,
                _machine.GetBoxContents().Aggregate((a, b) => a >= _machine.Goal ? a : a + b) + 1];
            if (possible.GetLength(1) - _machine.Goal <= 0)
            {
                _machine.Goal -= _machine.GetBoxContents().Where(a => a > 0).Min();
                return (Array.IndexOf(_machine.GetBoxContents(), _machine.GetBoxContents().Where(a=> a>0).Min()));
            }

            //Berechnen des Hauptproblems
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < possible.GetLength(1); j++)
                possible[i, j] = _machine.GetBoxContents()[i] == j || i > 0 &&
                                 (possible[i - 1, j] || j - _machine.GetBoxContents()[i] >= 0 &&
                                  possible[i - 1, j - _machine.GetBoxContents()[i]]);
            //Prüfen, was die bestmögliche Anzahl ist:
            var found = Enumerable.Range(_machine.Goal, possible.GetLength(1) - _machine.Goal).FirstOrDefault(a => possible[9, a]);
            //Benutzte Fächer rekonstruieren
            var used = new List<int>();
            for (var i = 9; i >= 0; i--)
            {
                if (_machine.GetBoxContents()[i] == found)
                {
                    used.Add(i);
                    break;
                }

                if (found - _machine.GetBoxContents()[i] >= 0 &&
                    possible[i - 1, found - _machine.GetBoxContents()[i]] &&
                    (i == 0 || !possible[i - 1, found]))
                {
                    used.Add(i);
                    found -= _machine.GetBoxContents()[i];
                }
            }

            //Fach mit dem kleinsten Inhalt auskippen
            _machine.Goal -= used.Select(i => _machine.GetBoxContents()[i]).Min();
            return (Array.IndexOf(_machine.GetBoxContents(),
                used.Select(i => _machine.GetBoxContents()[i]).Min()));
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