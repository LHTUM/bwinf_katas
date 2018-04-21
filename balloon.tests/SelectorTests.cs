using balloon.Selectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace balloon.tests
{
    [TestClass]
    public class SelectorTests
    {
        private ManualSelector _manualSelector;
        private BalloonMachine _balloonMachine;

        [TestInitialize]
        public void Initialize()
        {
            _manualSelector = new ManualSelector();
            _balloonMachine = new BalloonMachine();
            _manualSelector.SetBalloonMachine(_balloonMachine);
        }

        [TestMethod]
        public void IsFinished_Finished_True()
        {
            Assert.IsTrue(_manualSelector.IsFinished());
        }

        [TestMethod]
        public void IsFinished_NonEmptyBoxes_False()
        {
            _balloonMachine.SetBoxContents(new int[] { 1 });
            Assert.IsFalse(_manualSelector.IsFinished());
        }

        [TestMethod]
        public void IsFinished_OverfullPackage_False()
        {
            _balloonMachine.SetBoxContents(new int[] { 22 });
            _balloonMachine.Take(0);

            Assert.IsFalse(_manualSelector.IsFinished());
        }
    }
}
