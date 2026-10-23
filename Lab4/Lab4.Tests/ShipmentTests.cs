using System;
using Lab4.Core;
using Xunit;

namespace Lab4.Tests
{
    public class ShipmentTests
    {
        [Fact]
        public void Letter_CalculateCost_ReturnsCorrectValue()
        {
            var letter = new Letter("L-1", 0.1, false);
            Assert.Equal(105m, letter.CalculateCost());
        }

        [Fact]
        public void Letter_Urgent_ReturnsCorrectValue()
        {
            var letter = new Letter("L-1", 0.1, true);
            Assert.Equal(157.5m, letter.CalculateCost());
        }

        [Fact]
        public void OversizedCargo_CalculateCost_ReturnsCorrectValue()
        {
            var cargo = new OversizedCargo("O-1", 10.0, 1.0, false);
            Assert.Equal(2600m, cargo.CalculateCost());
        }

        [Fact]
        public void Parcel_CalculateCost_ReturnsCorrectValue()
        {
            var parcel = new Parcel("P-1", 2.0, false);
            Assert.Equal(500m, parcel.CalculateCost());
        }
        [Fact]
        public void Letter_EmptyTrackingNumber_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => new Letter("", 1.0, false));
        }
        [Fact]
        public void Parcel_NegativeWeight_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Parcel("P-1", -5.0, false));
        }
        [Fact]
        public void OversizedCargo_NegativeVolume_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new OversizedCargo("O-1", 10.0, -1.0, false));
        }
    }
}