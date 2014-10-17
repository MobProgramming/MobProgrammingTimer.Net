using System;
using System.Collections.Generic;
using ApprovalTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeveloperTimer.Tests
{
    [TestClass]
    public class EnumerableRandomizerShould
    {
        [TestMethod]
        public void ShouldRandomizeEnumerable()
        {
            IEnumerable<string> values = new[] { "one", "two", "three", "four", };

            var randomizer = new EnumerableRandomizer(new Random(2));

            Approvals.VerifyAll(randomizer.Randomize(values), "Result");
        } 
    }
}