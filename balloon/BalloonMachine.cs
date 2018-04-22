using System;
using System.Collections.Generic;
using System.Linq;

namespace balloon
{
    public class BalloonMachine
    {
        public int NumberOfBoxes = 10;
        public int Goal = 20;

        private Queue<int> _unprocessedBalloons;
        private readonly int[] _boxContents = new int[10];
        private int _packageContent;

        private readonly List<int> _packSizeHistory;

        public BalloonMachine()
        {
            _packSizeHistory = new List<int>();
            _unprocessedBalloons = new Queue<int>();
        }

        public void Take(int boxIndex)
        {
            var boxContent = _boxContents[boxIndex];
                _packageContent += boxContent;
            if (_unprocessedBalloons.Count > 0)
            {
                _boxContents[boxIndex] = _unprocessedBalloons.Dequeue();
            }
            else
            {
                _boxContents[boxIndex] = 0;
            }
        }

        public void Pack()
        {
            _packSizeHistory.Add(_packageContent);
            _packageContent = 0;
        }

        public int[] GetBoxContents()
        {
            return _boxContents;
        }

        public int GetPackageContent()
        {
            return _packageContent;
        }

        public List<int> GetPackSizeHistory()
        {
            return _packSizeHistory;
        }

        public void PrintStatistics()
        {
            Dictionary<int, int> packageContentCounts = new Dictionary<int, int>();
            Console.Out.WriteLine("--------------");
            Console.Out.WriteLine("Statistics:");
            while (_packSizeHistory.Count>0)
            {
                var content = _packSizeHistory.First();
                var count = 0;
                foreach(var packageContent in _packSizeHistory)
                {
                    if(packageContent == content)
                    {
                        count++;
                    }
                }
                packageContentCounts.Add(content, count);
                _packSizeHistory.RemoveAll(c => c == content);
            }
            List<int> counts = new List<int>(packageContentCounts.Keys);
            counts.Sort();
            var s = "";
            foreach(var count in counts)
            {
                s += ("( " + count + " ) x " + packageContentCounts[count] + "\t");
            }
            s += "Rest : " + _packageContent;
            Console.Out.WriteLine(s);
            Console.Out.WriteLine("--------------");
        }

        public void PrintCurrentStatus()
        {
            Console.Out.WriteLine("Current Status:");
            Console.Out.WriteLine("--------------");
            Console.Out.WriteLine("Box contents: " + GetBoxContentsAsString());
            Console.Out.WriteLine("Package content:" + GetPackageContent());
            Console.Out.WriteLine("--------------");
        }

        private string GetBoxContentsAsString()
        {
            var s = "";
            foreach(var content in _boxContents)
            {
                s += ", " + content;
            }
            return s.Substring(2,s.Length-2);
        }

        public void SetBoxContents(int[] contents)
        {
            if (contents != null)
            {
                for (var i = 0; i < _boxContents.Length; i++)
                {
                    if (contents.Length > i)
                    {
                        _boxContents[i] = contents[i];
                    }
                }
            }
        }

        public void SetBalloonQueue(int[] balloons)
        {
            _unprocessedBalloons = new Queue<int>();
            foreach (var ballonCount in balloons)
            {
                _unprocessedBalloons.Enqueue(ballonCount);
            }

            if (!HasEmptyBoxes()) return;
            for (var i = 0; i < NumberOfBoxes; i++)
            {
                Take(i);
            }
        }

        public bool HasEmptyBoxes()
        {
            foreach(var boxContent in _boxContents)
            {
                if (boxContent != 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
