using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using ApprovalTests.Reporters;
using ApprovalTests.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace DeveloperTimer.Tests
{
    [TestClass]
    [UseReporter(typeof(AllFailingTestsClipboardReporter))]
    public class Explortitory
    {
        private readonly DateTime touDate = DateTime.Parse("6/28/3185 3:07:18AM");

        [TestMethod]
        public void ConstructorTest()
        {
            var timeController = CreateTimeControllerParts(() => touDate).Item1;
            var mainWindow = BuildMainWindow(timeController);
            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ResetTimerTest()
        {
            var timeController = CreateTimeControllerParts(() => touDate).Item1;

            var mainWindow = BuildMainWindow(timeController);
            mainWindow.ResetTimer();
            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ProgressTimerTest()
        {
            var timeControllerParts = CreateTimeControllerParts(() => touDate);
            var timeController = timeControllerParts.Item1;

            var mainWindow = BuildMainWindow(timeController);
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

            var mainWindow = BuildMainWindow(timeController);
            mainWindow.ResetTimer();
            timeControllerParts.Item2[0](timeController, null);

            WpfApprovals.Verify(mainWindow);
        }

        [TestMethod]
        public void ProgressTimerToEnd()
        {
            var dateTimes = GetInitialDateTimes(); 
            dateTimes.Enqueue(touDate.AddMinutes(17).AddSeconds(22));

            var timeControllerParts = CreateTimeControllerParts(dateTimes.Dequeue);
            var timeController = timeControllerParts.Item1;

            var mainWindow = BuildMainWindow(timeController);
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

        private static MainWindow BuildMainWindow(ITimeController timeController)
        {
            var mainWindow = new MainWindow(timeController, IgnoreQueryUserBeforeResetTimer(), () => false);
            return mainWindow;
        }

        private static Func<string, MessageBoxResult> IgnoreQueryUserBeforeResetTimer()
        {
            return s => MessageBoxResult.Yes;
        }
    }
}
