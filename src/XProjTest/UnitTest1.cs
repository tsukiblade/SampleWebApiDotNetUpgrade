using FluentAssertions;
using NUnit.Framework;
using Xunit;

namespace XProjTest
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void Test1()
        {
            "xd".Should().Be("xd");
        }
    }
}