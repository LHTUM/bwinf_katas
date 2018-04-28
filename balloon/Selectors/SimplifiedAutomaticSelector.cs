using System;
using System.Collections.Generic;
using System.Linq;

namespace balloon.Selectors
{
    public class SimplifiedAutomaticSelector : ISelector
    {
        private BalloonMachine _machine;

        private bool[,] _possiblePackageContents;
        private int _missingContent;
        private int _accumulatedBoxContents;

        public void ExecuteStep()
        {
            if (IsReadyToPack())
                {
                    _machine.Pack();
                }
            else
            {
                var box = ProvideBox();
                _machine.Take(box);
            }
        }

        private bool IsReadyToPack()
        {
            return _machine.GetPackageContent() >= _machine.Goal;
        }

        public int ProvideBox()
        {   
            if (_accumulatedBoxContents < _missingContent)
            {
                return GetIndexOfBoxWithSmallestNonZeroContent();
            }
            
            CalculatePossiblePackageContents();
            var bestPackageContent = DetermineBestPackageContent();
            var usedBoxes = DetermineUsedBoxes(bestPackageContent);
            var boxIndex = DetermineBestBoxIndex(usedBoxes);
            return (boxIndex);
        }

        private int GetIndexOfBoxWithSmallestNonZeroContent()
        {
            return GetIndexOfSmallestNonZeroEntry(_machine.GetBoxContents());
        }

        private int DetermineBestBoxIndex(List<int> usedBoxes)
        {
            if (usedBoxes.Count == 0)
            {
                usedBoxes.Add(GetIndexOfSmallestNonZeroEntry(_machine.GetBoxContents()));
            }

            var result = Array.IndexOf(_machine.GetBoxContents(),
                usedBoxes.Select(i => _machine.GetBoxContents()[i]).Min());
            return result;
        }

        private List<int> DetermineUsedBoxes(int bestPackageContent)
        {
            var missingContentForBestPackage = bestPackageContent;
            var usedBoxes = new List<int>();
            for (var i = _machine.NumberOfBoxes - 1; i >= 0; i--)
            {
                var currentBoxContent = _machine.GetBoxContents()[i];

                if (currentBoxContent == missingContentForBestPackage)
                {
                    usedBoxes.Add(i);
                    break;
                }

                if (i == 0 || !IsPossibleWithoutCurrentBox(i, missingContentForBestPackage))
                {
                    usedBoxes.Add(i);
                    missingContentForBestPackage -= currentBoxContent;
                }
            }

            return usedBoxes;
        }

        private int DetermineBestPackageContent()
        {
            var bestPackageContent = _accumulatedBoxContents;
            for (var i = _missingContent; i <= _accumulatedBoxContents; i++)
            {
                if (_possiblePackageContents[_machine.NumberOfBoxes - 1, i])
                {
                    bestPackageContent = i;
                    break;
                }
            }
            return bestPackageContent;
        }

        private void CalculatePossiblePackageContents()
        {
            var maximum = _accumulatedBoxContents + 1;
            _possiblePackageContents =
                new bool[_machine.NumberOfBoxes, maximum]; // rows: included boxes, cols: intended sum 
            for (var i = 0; i < _machine.NumberOfBoxes; i++)
            {
                for (var intendedPackageContent = 0;
                    intendedPackageContent <= maximum;
                    intendedPackageContent++)
                {
                    var currentBoxConent = _machine.GetBoxContents()[i];
                    var includedBoxes = i;
                    if (currentBoxConent == intendedPackageContent ||
                        i > 0 && (IsPossibleWithoutCurrentBox(i, intendedPackageContent) ||
                                  intendedPackageContent > currentBoxConent &&
                                  IsPossibleWithoutCurrentBox(i, intendedPackageContent - currentBoxConent)))
                    {
                        _possiblePackageContents[includedBoxes, intendedPackageContent] = true;
                    }
                }
            }
        }

        public bool IsFinished()
        {
            return _machine.HasEmptyBoxes() && _machine.GetPackageContent() < _machine.Goal;
        }

        public void SetBalloonMachine(BalloonMachine balloonMachine)
        {
            _machine = balloonMachine;
            _missingContent = _machine.Goal - _machine.GetPackageContent();
            _accumulatedBoxContents = _machine.GetBoxContents().Sum();
        }

        public static int GetIndexOfSmallestNonZeroEntry(int[] array)
        {
            return Array.IndexOf(array, array.Where(entry => entry > 0).Min());
        }

        private bool IsPossibleWithoutCurrentBox(int currentBoxIndex, int intendetContent)
        {
            return _possiblePackageContents[currentBoxIndex - 1, intendetContent];
        }
    }
}
