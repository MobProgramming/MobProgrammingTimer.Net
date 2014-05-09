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
            GetRandomNumber randomNumberGenerator = max => max >= 2 ? 2 : max;

            IEnumerable<string> values = new[] { "one", "two", "three", "four", };

            var randomizer = new EnumerableRandomizer(randomNumberGenerator);

            Approvals.VerifyAll(randomizer.Randomize(values), "Result");
        } 
    }
}