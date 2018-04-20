using System;
using System.Collections.Generic;
using System.Linq;

namespace balloon
{
    public class BalloonMachine
    {
        private const int _numberOfBoxes = 10;
        private int[] _boxes = new int[10];

        private List<int> _packSizeHistory { get; set; }
        private int _currentPackSize { get; set; }
        
        private Stack<int> _unprocessedBalloons;

        public BalloonMachine()
        {
            _packSizeHistory = new List<int>();
            _unprocessedBalloons = new Stack<int>();
        }

        public int[] getCurrentBoxContents()
        {
            return _boxes;
        }

        public int GetCurrentPackageContent()
        {
            return _currentPackSize;
        }

        public List<int> GetPackSizeHistory()
        {
            return _packSizeHistory;
        }

        public void Pack()
        {
            _packSizeHistory.Add(_currentPackSize);
            _currentPackSize = 0;
        }

        public void PrintStatistics()
        {
            Dictionary<int, int> packageContentCounts = new Dictionary<int, int>();
            Console.Out.WriteLine("--------------");
            while(_packSizeHistory.Count>0)
            {
                int content = _packSizeHistory.First();
                int count = 0;
                foreach(int packageContent in _packSizeHistory)
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
            foreach(int count in counts)
            {
                Console.Out.WriteLine(count + ":" + packageContentCounts[count]);
            }
            Console.Out.WriteLine("--------------");
        }

        public void Take(int boxIndex)
        {
            int boxContent = _boxes[boxIndex];
            try
            {
                _currentPackSize += boxContent;
                _boxes[boxIndex] = _unprocessedBalloons.Pop();
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
    }
}
