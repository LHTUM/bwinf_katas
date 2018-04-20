using System;
using System.Collections.Generic;
using System.Text;

namespace balloon
{
    public interface IBalloonMachine
    {
        List<int> resultingPackSizes { get; set; }
        int lastPackSize { get; set; }

        int[] getCurrentBoxContents();
        int getCurrentPackageContent();
        void printStatistics();
        void pack();
        void take(int boxIndexe);
    }
}
