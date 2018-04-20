using System;
using System.Collections.Generic;
using System.Text;

namespace balloon
{
    class BallonoMachine : IBalloonMachine
    {
        private Queue<int> _unprocessedBalloons;
        private List<int> _resultingPackSizes;
        private int _lastPackSize;

        public int[] getCurrentBoxContents()
        {
            throw new NotImplementedException();
        }

        public int getCurrentPackageContent()
        {
            throw new NotImplementedException();
        }

        public void pack()
        {
            throw new NotImplementedException();
        }

        public void printStatistics()
        {
            throw new NotImplementedException();
        }

        public void take(int boxIndexe)
        {
            throw new NotImplementedException();
        }
    }
}
