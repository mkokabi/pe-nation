using System;
using Xunit;
using APIMocker;

namespace APIMocker.Tests
{
    public class QueryStringUnitTests
    {
        [Fact]
        public void TestSimpleOneSegment()
        {
            Assert.True(QueryStringChecker.Match("?customerNo={0}", "?customerNo=12345"));
        }

        [Fact]
        public void TestTwoSegments()
        {
            Assert.True(QueryStringChecker.Match("?customerNo={0}&year={1}", "?customerNo=12345&year=2020"));
        }

        [Fact]
        public void TestTwoSegmentsWithSomeValues()
        {
            Assert.True(QueryStringChecker.Match("?customerNo={0}&year=2020", "?customerNo=12345&year=2020"));
        }
    }
}
