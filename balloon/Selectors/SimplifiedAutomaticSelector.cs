using System;
using System.Collections.Generic;
using System.Linq;

namespace balloon.Selectors
{
    public class SimplifiedAutomaticSelector : ISelector
    {
        private BalloonMachine _machine;
        private bool[,] _possiblePackageContents;

        public void Run()
        {
            while (!IsFinished())
            {
                if (_machine.GetPackageContent() >= _machine.Goal)
                {
                    _machine.Pack();
                }
                else
                {
                    var box = ProvideBox();
                    _machine.Take(box);
                }
            }
        }

        public int ProvideBox()
        {
            var missingContent = _machine.Goal - _machine.GetPackageContent();
            var accumulatedBoxContents = _machine.GetBoxContents().Sum();
            if (accumulatedBoxContents < missingContent)
            {
                return (GetIndexOfSmallestNonZeroEntry(_machine.GetBoxContents()));
            }
            
            _possiblePackageContents = new bool[_machine.NumberOfBoxes, accumulatedBoxContents+1]; // rows: included boxes, cols: intended sum 
            for (var i = 0; i < _machine.NumberOfBoxes; i++)
            {
                for (var intendedPackageContent = 0;
                    intendedPackageContent <= accumulatedBoxContents;
                    intendedPackageContent++)
                {
                    var currentBoxConent = _machine.GetBoxContents()[i];
                    var includedBoxes = i;
                    if (currentBoxConent == intendedPackageContent ||
                        i > 0 && (IsPossibleWithoutCurrentBox(i, intendedPackageContent) ||
                        intendedPackageContent > currentBoxConent && IsPossibleWithoutCurrentBox(i, intendedPackageContent - currentBoxConent)))
                    {
                        _possiblePackageContents[includedBoxes, intendedPackageContent] = true;
                    }
                }
            }

            var bestPackageContent = accumulatedBoxContents;
            for (var i = missingContent; i <= accumulatedBoxContents; i++)
            {
                if (_possiblePackageContents[_machine.NumberOfBoxes - 1, i])
                {
                    bestPackageContent = i;
                    break;
                }
            }

            var missingContentForBestPackage = bestPackageContent;
            var usedBoxes = new List<int>();
            for (var i = _machine.NumberOfBoxes-1; i >= 0; i--)
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

            if (usedBoxes.Count == 0)
            {
                usedBoxes.Add(GetIndexOfSmallestNonZeroEntry(_machine.GetBoxContents()));
            }
            return (Array.IndexOf(_machine.GetBoxContents(), usedBoxes.Select(i => _machine.GetBoxContents()[i]).Min()));
        }


        public bool IsFinished()
        {
            return _machine.HasEmptyBoxes() && _machine.GetPackageContent() < _machine.Goal;
        }

        public void SetBalloonMachine(BalloonMachine balloonMachine)
        {
            _machine = balloonMachine;
        }

        private int GetIndexOfSmallestNonZeroEntry(int[] array)
        {
            return Array.IndexOf(array, array.Where(entry => entry > 0).Min());
        }

        private bool IsPossibleWithoutCurrentBox(int currentBoxIndex, int intendetContent)
        {
            return _possiblePackageContents[currentBoxIndex - 1, intendetContent];
        }
    }
}