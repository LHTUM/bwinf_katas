using System;
using System.Collections.Generic;
using System.Linq;

namespace balloon
{
    public class BalloonMachine
    {
        public int NumberOfBoxes = 10;
        public int Goal = 20;
        private readonly int[] _boxes = new int[10];

        private List<int> PackSizeHistory { get; set; }
        private int CurrentPackSize { get; set; }
        
        private Queue<int> _unprocessedBalloons;

        public BalloonMachine()
        {
            PackSizeHistory = new List<int>();
            _unprocessedBalloons = new Queue<int>();
        }

        public int[] GetCurrentBoxContents()
        {
            return _boxes;
        }

        public int GetCurrentPackageContent()
        {
            return CurrentPackSize;
        }

        public List<int> GetPackSizeHistory()
        {
            return PackSizeHistory;
        }

        public void Pack()
        {
            PackSizeHistory.Add(CurrentPackSize);
            CurrentPackSize = 0;
        }

        public void PrintStatistics()
        {
            Dictionary<int, int> packageContentCounts = new Dictionary<int, int>();
            Console.Out.WriteLine("--------------");
            Console.Out.WriteLine("Statistics:");
            while (PackSizeHistory.Count>0)
            {
                int content = PackSizeHistory.First();
                int count = 0;
                foreach(int packageContent in PackSizeHistory)
                {
                    if(packageContent == content)
                    {
                        count++;
                    }
                }
                packageContentCounts.Add(content, count);
                PackSizeHistory.RemoveAll(c => c == content);
            }
            List<int> counts = new List<int>(packageContentCounts.Keys);
            counts.Sort();
            foreach(int count in counts)
            {
                Console.Out.WriteLine(count + ": " + packageContentCounts[count]);
            }
            Console.Out.WriteLine("--------------");
        }

        public void PrintCurrentStatus()
        {
            Console.Out.WriteLine("Current Status:");
            Console.Out.WriteLine("--------------");
            Console.Out.WriteLine("Box contents: " + GetBoxContentsAsString());
            Console.Out.WriteLine("Package content:" + GetCurrentPackageContent());
            Console.Out.WriteLine("--------------");
        }

        private string GetBoxContentsAsString()
        {
            string s = "";
            foreach(int content in _boxes)
            {
                s += ", " + content;
            }
            return s.Substring(2,s.Length-2);
        }

        public void Take(int boxIndex)
        {
            int boxContent = _boxes[boxIndex];
            try
            {
                CurrentPackSize += boxContent;
                _boxes[boxIndex] = _unprocessedBalloons.Dequeue();
            } catch
            {
                _boxes[boxIndex] = 0;
            }
        }

        public void SetBoxContents(int[] contents)
        {
            if (contents != null)
            {
                for (int i = 0; i < _boxes.Length; i++)
                {
                    if (contents.Length > i)
                    {
                        _boxes[i] = contents[i];
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
            foreach(int boxContent in _boxes)
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
