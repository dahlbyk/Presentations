using Interfaces;
using NUnit.Framework;
using Simple;

namespace Tests
{
    [TestFixture]
    public class FizzBuzzTests
    {
        private IKnowFizzBuzz fb;

        [SetUp]
        public void SetUp()
        {
            fb = new FizzBuzz();
        }

        [Test]
        public void Simple()
        {
            var actual = fb.Translate(1);

            Assert.AreEqual("1", actual);
        }
    }
}
