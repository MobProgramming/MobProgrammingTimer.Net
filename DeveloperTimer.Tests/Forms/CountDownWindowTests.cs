using System;
using System.Collections;
using System.Collections.Generic;
using ApprovalTests.Wpf;
using ApprovalUtilities.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTimer.Tests.Forms
{
    [TestClass]
    public class CountDownWindowTests
    {
        [TestMethod]
        public void TestPassiveCountDownWindow()
        {
            ILoader<DateTime> loaderTime = new GenericLoader<DateTime>(() => DateTime.Parse("6:20PM January 1, 2000"));
            var window = new CountDownWindow(3, 14, loaderTime);
            WpfApprovals.Verify(window);
        }

        [TestMethod]
        public void TestPassiveCountDownWindowCountsDown()
        {
            var times = new Stack<DateTime>( new[]{ DateTime.Parse("6:21PM January 1, 2000"), DateTime.Parse("6:20PM January 1, 2000") });
            ILoader<DateTime> loaderTime = new GenericLoader<DateTime>(() => times.Count > 1? times.Pop():times.Peek());
            var window = new CountDownWindow(3, 14, loaderTime);
            WpfApprovals.Verify(window);
        }
    }

    public class GenericLoader<T> : ILoader<T>
    {
        private readonly Func<T> loadFunc;

        public GenericLoader(Func<T> loadFunc)
        {
            this.loadFunc = loadFunc;
        }

        public T Load()
        {
            return loadFunc();
        }
    }
}