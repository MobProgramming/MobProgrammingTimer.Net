using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using System.Windows;
using ApprovalTests;
using ApprovalTests.Wpf;
using ApprovalUtilities.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace DeveloperTimer.Tests
{
    [TestClass]
    public class UiTests
    {
        private readonly DateTime touDate = DateTime.Parse("6/28/3185 3:07:18AM");

        [TestMethod]
        public void ConstructorTest()
        {
            var timeController = CreateTimeControllerParts(() => touDate).Item1;
            var mainWindow = BuildMainWindow(timeController, new ItemRing<string>());
            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ResetTimerTest()
        {
            var timeController = CreateTimeControllerParts(() => touDate).Item1;

            var mainWindow = BuildMainWindow(timeController, new ItemRing<string>());
            mainWindow.ResetTimer();
            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ProgressTimerTest()
        {
            var timeControllerParts = CreateTimeControllerParts(() => touDate);
            var timeController = timeControllerParts.Item1;

            var mainWindow = BuildMainWindow(timeController, new ItemRing<string>());
            mainWindow.ResetTimer();

            timeControllerParts.Item2[0](timeController, null);
            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ProgressTimerToNewTimeTest()
        {
            var dateTimes = GetInitialDateTimes();
            dateTimes.Enqueue(touDate.AddMinutes(2));

            var timeControllerParts = CreateTimeControllerParts(dateTimes.Dequeue);
            var timeController = timeControllerParts.Item1;

            var mainWindow = BuildMainWindow(timeController, new ItemRing<string>());
            mainWindow.ResetTimer();
            timeControllerParts.Item2[0](timeController, null);

            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void TestListDisplayUpdatesWhenListUpdates()
        {
            var dateTimes = GetInitialDateTimes();
            dateTimes.Enqueue(touDate.AddMinutes(2));

            var timeControllerParts = CreateTimeControllerParts(dateTimes.Dequeue);
            var timeController = timeControllerParts.Item1;

            var itemRing = new ItemRing<string>(){"One", "Two", "Three"};
            var mainWindow = BuildMainWindow(timeController, itemRing);
            itemRing.RemoveAt(1);
            timeControllerParts.Item2[0](timeController, null);

            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void TestLoadExistingFile()
        {
            ILoader<bool> mockFileExistLoader = new MockLoader<bool>(true);
            var streamReader = new StreamReader(new MemoryStream());
            ILoader<StreamReader> mockFileLoader = new Loader<StreamReader>(streamReader);
            Func<Stream> mockCreater = () =>
            {
                throw new Exception();
            };
            using (
                var testableGetnamesFile = MainWindow.TestableGetnamesFile(mockFileExistLoader, mockFileLoader,
                    mockCreater))
            {
                Assert.AreEqual(streamReader, testableGetnamesFile);
            }
        }

        [TestMethod]
        public void TestLoadNonExistingFile()
        {
            var wasCalled = false;
            ILoader<bool> mockFileExistLoader = new MockLoader<bool>(false);
            var streamReader = new StreamReader(new MemoryStream());
            ILoader<StreamReader> mockFileLoader = new Loader<StreamReader>(streamReader);
            Func<Stream> mockCreater = () =>
            {
                wasCalled = true;
                return null;
            };
            using (
                var testableGetnamesFile = MainWindow.TestableGetnamesFile(mockFileExistLoader, mockFileLoader,
                    mockCreater))
            {
                Assert.AreEqual(streamReader, testableGetnamesFile);
            }
            Assert.IsTrue(wasCalled,"Failed to create");
        }

        [TestMethod]
        public void ProgressTimerToEnd()
        {
            var dateTimes = GetInitialDateTimes(); 
            dateTimes.Enqueue(touDate.AddMinutes(17).AddSeconds(22));

            var timeControllerParts = CreateTimeControllerParts(dateTimes.Dequeue);
            var timeController = timeControllerParts.Item1;

            var mainWindow = BuildMainWindow(timeController, new ItemRing<string>());
            mainWindow.ResetTimer();
            timeControllerParts.Item2[0](timeController, null);

            WpfApprovals.Verify(mainWindow);
        }

        private Tuple<ITimeController, IList<Action<object, ElapsedEventArgs>>> CreateTimeControllerParts(Func<DateTime> now)
        {
            var timeController = MockRepository.GenerateStub<ITimeController>();

            timeController.Stub(tc => tc.Now).Return(touDate).WhenCalled(mi => mi.ReturnValue = now());
            timeController.DelaySpan = new TimeSpan(0, 15, 0);

            var argumentsForCallsMadeOn = timeController.Capture().Args<Action<object, ElapsedEventArgs>>((tc, action) => tc.Listen(action));

            return Tuple.Create(timeController, argumentsForCallsMadeOn);
        }

        private Queue<DateTime> GetInitialDateTimes()
        {
            var dateTimes = new Queue<DateTime>();
            dateTimes.Enqueue(touDate);
            dateTimes.Enqueue(touDate);
            dateTimes.Enqueue(touDate);
            return dateTimes;
        }

        private static MainWindow BuildMainWindow(ITimeController timeController, ItemRing<string> itemRing)
        {
            var mainWindow = new MainWindow(timeController, IgnoreQueryUserBeforeResetTimer(), () => false, itemRing);
            return mainWindow;
        }

        private static Func<string, MessageBoxResult> IgnoreQueryUserBeforeResetTimer()
        {
            return s => MessageBoxResult.Yes;
        }
    }
}
