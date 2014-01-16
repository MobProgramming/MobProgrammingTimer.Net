using System;
using ApprovalTests.Wpf;
using DeveloperTimer.Models;
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

        [TestMethod]
        public void TestDisplayNames()
        {
            //  1.  A list of users
            //  2.  a nullable pointer to a user
            
            //  create user model with a list of names
            UsersModel model = new UsersModel(new []
            {
                "user1",
                "user2",
                "user3"
            });

            //  create the main window with the created view model
            var window = new MainWindow(model);
            
            //  verify
            WpfApprovals.Verify(window);
        }
    }
}
