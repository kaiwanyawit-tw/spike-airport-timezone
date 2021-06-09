using FluentAssertions;
using NodaTime;
using NUnit.Framework;

namespace AirportToTimeZone.Tests
{
    public class TimeZoneServiceTests
    {
        private TimeZoneService _target;

        [SetUp]
        public void Setup()
        {
            _target = new TimeZoneService();
        }

        [Test]
        public void ShouldCreate()
        {
            _target.Should().NotBeNull();
        }

        [Test]
        [TestCase(true, "NRT")]
        [TestCase(false, "XXX")]
        public void ContainsAirportCodeShouldReturn(bool expected, string airportCode)
        {
            var result = _target.ContainsAirportCode(airportCode);
            result.Should().Be(expected);
        }

        [Test]
        [TestCase("Asia/Tokyo", "NRT")]
        [TestCase("Asia/Bangkok", "BKK")]
        public void FromAirportCodeShouldReturnTimeZone(string expected, string airportCode)
        {
            var result = _target.FromAirportCode(airportCode);
            result.timeZone.Should().Be(expected);
        }

        [Test]
        [TestCase(+9, "NRT")]
        [TestCase(+7, "BKK")]
        public void FromAirportCodeShouldReturnOffset(int expected, string airportCode)
        {
            var result = _target.FromAirportCode(airportCode);
            result.offset.Should().Be(Offset.FromHours(expected));
        }
    }
}