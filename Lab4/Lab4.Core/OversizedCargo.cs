using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Core
{
    public class OversizedCargo : Shipment
    {
        public double Volume { get; }
        public OversizedCargo(string trackingNumber, double weight, double volume, bool isUrgent)
            : base(trackingNumber, weight, isUrgent)
        {
            if (volume <= 0)
                throw new ArgumentOutOfRangeException(nameof(volume), "־בתול המכזום בûעü במכüרו 0");
            Volume = volume;
        }
        public override decimal CalculateCost()
        {
            decimal baseCost = 100m + (decimal)Weight * 200m + (decimal)Volume * 500m;
            if (IsUrgent)
                baseCost *= 1.8m;
            return baseCost;
        }
        public override string ToString()
        {
            return base.ToString() + $", ־בתול {Volume} ל^3";
        }
    }
}
