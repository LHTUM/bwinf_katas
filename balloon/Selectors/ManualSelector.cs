using System;

namespace balloon.Selectors
{
    public class ManualSelector
    {
        private BalloonMachine _machine;
        
        public void Run()
        {
            _machine = new BalloonMachine();
            _machine.SetBoxContents(new[]{1, 2, 3, 4, 5, 6, 7, 8, 9, 10});
            _machine.SetBalloonQueue(new[] {5,5,5});

            while(!IsFinished())
            {
                PrintTask();
                int action = ProvideAction();
                if (action == -1)
                {
                    _machine.Pack();
                } else
                {
                    _machine.Take(action);
                }
            }
            _machine.PrintStatistics();
            Console.In.Read();
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

        public void PrintTask()
        {
            _machine.PrintCurrentStatus();
            Console.Out.WriteLine("Please select a box.");           
        }

        private int ProvideAction()
        {
            int selectedBoxIndex = int.MaxValue;
            while (selectedBoxIndex > _machine.NumberOfBoxes && selectedBoxIndex >= -1)
            {
                try
                {
                    selectedBoxIndex = int.Parse(Console.In.ReadLine());
                }
                catch
                {
                    // ignored
                }
            }
            return selectedBoxIndex;
        }
    }
}
