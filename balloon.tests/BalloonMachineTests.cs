using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace balloon.tests
{
    [TestClass]
    public class BalloonMachineTests
    {
        private BalloonMachine _machine;
        [TestInitialize]
        public void Initialize()
        {
            _machine = new BalloonMachine();
        }

        [TestMethod]
        public void Take_FirstFilledBox_EmptyBoxAndFilledPackage()
        {
            _machine.SetBoxContents(new int[] {1});

            _machine.Take(0);

            Assert.IsTrue(_machine.getCurrentBoxContents()[0] == 0);
            Assert.IsTrue(_machine.GetCurrentPackageContent() == 1);
        }

        [TestMethod]
        public void Take_FilledFirstAndSecondBox_FilledFirstAndEmptySecondBoxAndFilledPackage()
        {
            _machine.SetBoxContents(new int[] { 1, 2 });

            _machine.Take(1);

            Assert.IsTrue(_machine.getCurrentBoxContents()[0] == 1);
            Assert.IsTrue(_machine.getCurrentBoxContents()[1] == 0);
            Assert.IsTrue(_machine.GetCurrentPackageContent() == 2);
        }

        [TestMethod]
        public void Pack_FilledPackage_PersistedAndEmptied()
        {
            _machine.SetBoxContents(new int[] { 2 });
            _machine.Take(0);
            _machine.Pack();

            Assert.IsTrue(_machine.GetCurrentPackageContent() == 0);
            Assert.IsTrue(_machine.GetPackSizeHistory().Count == 1);
            Assert.IsTrue(_machine.GetPackSizeHistory().Contains(2));
        }
    }
}
