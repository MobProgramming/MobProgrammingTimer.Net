using System;
using ApprovalTests.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTimer.Tests.Forms
{
    [TestClass]
    public class MainWindowTests
    {
        [TestMethod]
        public void TestMainWindow()
        {
            var window = new MainWindow();
            WpfApprovals.Verify(window);
        }
    }
}
