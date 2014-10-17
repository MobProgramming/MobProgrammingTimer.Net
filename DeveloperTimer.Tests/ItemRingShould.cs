using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalUtilities.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTimer.Tests
{
    [TestClass]
    public class ItemRingShould
    {
        private readonly string[] items = new[] {"one", "two", "three"};

        [TestMethod]
        public void ShouldSaveItemsToSaver()
        {
            var itemRing = new ItemRing<string>(items);
            var saver = new MockSaver<IEnumerable<string>>();
            itemRing.Save(saver);
            Approvals.VerifyAll(saver.Saved.First(), "Saved");
        }

        [TestMethod]
        public void ShouldTellWhatsInTheRing()
        {
            var itemRing = new ItemRing<string>(items);

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethod]
        public void ShouldStartOffIndicatingTopOfListAsCurrent()
        {
            var itemRing = new ItemRing<string>(items);

            Assert.AreEqual(items[0], itemRing.Current);
        }

        [TestMethod]
        public void ShouldStartOffIndicatingTheSecondItemIsNext()
        {
            var itemRing = new ItemRing<string>(items);

            Assert.AreEqual(items[1], itemRing.Next);
        }

        [TestMethod]
        public void ShouldNotChangeListAfterIncrement()
        {
            var itemRing = new ItemRing<string>(items);

            itemRing.Increment();

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethod]
        public void ShouldIndicateSecondItemAsCurrentAfterIncrement()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();

            Assert.AreEqual(items[1], itemRing.Current);
        }

        [TestMethod]
        public void ShouldIndicateThirdItemAsNextAfterIncrement()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();

            Assert.AreEqual(items[2], itemRing.Next);
        }

        [TestMethod]
        public void ShouldIndicateFirstItemIsNextWhenCurrentIsLastItem()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();
            itemRing.Increment();

            Assert.AreEqual(items[0], itemRing.Next);
        }

        [TestMethodAttribute]
        public void ShouldRotateBackToFirstItemOnOverFlow()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();
            itemRing.Increment();
            itemRing.Increment();

            Assert.AreEqual(items.First(), itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldAddNewItemToEndOfList()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Add("four");

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethodAttribute]
        public void ShouldIncrementCurrentIfCurrentIsRemovedByIndex()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.RemoveAt(0);

            Assert.AreEqual(items[1], itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldIncrementCurrentIfCurrentIsRemoved()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Remove(items.First());

            Assert.AreEqual(items[1], itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldCorrectlyIncrementCurrentIfCurrentIsLastAndRemoved()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();
            itemRing.Increment();

            itemRing.Remove(itemRing.Current);

            Assert.AreEqual(items.First(), itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldCorrectlyIncrementCurrentIfCurrentIsLastAndRemovedByIndex()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();
            itemRing.Increment();

            itemRing.RemoveAt(2);

            Assert.AreEqual(items.First(), itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldCorretlyPointToCurrentItemIfItemIsRemovedBefore()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();

            itemRing.Remove(items.First());

            Assert.AreEqual(items[1], itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldCorretlyPointToCurrentItemIfItemIsRemovedBeforeByIndex()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.Increment();

            itemRing.RemoveAt(0);

            Assert.AreEqual(items[1], itemRing.Current);
        }

        [TestMethodAttribute]
        public void ShouldChangeListIfFirstItemIsMovedDown()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.MoveDown(items.First());

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethodAttribute]
        public void ShouldCorrectlyWrapOnOnverFlowCausedByMovingDown()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.MoveDown(items.Last());

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethodAttribute]
        public void ShouldChangeListIfLastItemIsMovedUp()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.MoveUp(items.Last());

            Approvals.VerifyAll(itemRing.Items, "Items");
        }

        [TestMethodAttribute]
        public void ShouldCorrectlyWrapOnOverFlowCausedByMovingUp()
        {
            var itemRing = new ItemRing<string>(items);
            itemRing.MoveUp(items.First());

            Approvals.VerifyAll(itemRing.Items, "Items");
        }
    }
}