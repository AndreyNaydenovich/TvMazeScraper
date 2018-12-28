using System;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Presentation.Domain.Comparers;

namespace TvMazeScraper.Presentation.Domain.Tests.Comparers
{
    public class CastComparerTest
    {
        private CastComparer _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new CastComparer();
        }

        [Test]
        public void Compare_BothNull()
        {
            var x = new Mock<ICast>();
            var y = new Mock<ICast>();

            var result = _sut.Compare(x.Object, y.Object);
            result.Should().Be(0);
        }

        [Test]
        public void Compare_FirstNull()
        {
            var x = new Mock<ICast>();
            var y = new Mock<ICast>();
            y.Setup(t => t.Birthday).Returns(DateTime.Now);

            var result = _sut.Compare(x.Object, y.Object);
            result.Should().Be(1);
        }

        [Test]
        public void Compare_SecondNull()
        {
            var x = new Mock<ICast>();
            x.Setup(t => t.Birthday).Returns(DateTime.Now);
            var y = new Mock<ICast>();

            var result = _sut.Compare(x.Object, y.Object);
            result.Should().Be(-1);
        }

        [Test]
        public void Compare_BothHasValue()
        {
            var x = new Mock<ICast>();
            x.Setup(t => t.Birthday).Returns(DateTime.Now.AddDays(1));
            var y = new Mock<ICast>();
            y.Setup(t => t.Birthday).Returns(DateTime.Now);

            var result = _sut.Compare(x.Object, y.Object);
            result.Should().Be(-1);

            result = _sut.Compare(y.Object, x.Object);
            result.Should().Be(1);
        }

        [Test]
        public void Compare_BothHasValue_Equal()
        {
            var date = DateTime.Now;
            var x = new Mock<ICast>();
            x.Setup(t => t.Birthday).Returns(date);
            var y = new Mock<ICast>();
            y.Setup(t => t.Birthday).Returns(date);

            var result = _sut.Compare(x.Object, y.Object);
            result.Should().Be(0);
        }
    }
}
