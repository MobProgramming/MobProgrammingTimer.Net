using ApprovalTests.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTimer.Tests.Forms
{
    [TestClass]
    public class CountDownWindowTests
    {
        [TestMethod]
        public void TestPassiveCountDownWindow()
        {
            var window = new CountDownWindow(3, 14); 
            WpfApprovals.Verify(window);
        }
    }
}