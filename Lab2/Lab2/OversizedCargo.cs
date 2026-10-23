using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class OversizedCargo : Shipment
    {
        public double Volume { get; }
        public OversizedCargo(string trackingNumber, double weight, double volume, bool isUrgent)
            : base(trackingNumber, weight, isUrgent)
        {
            if (volume <= 0)
                throw new ArgumentOutOfRangeException(nameof(volume), "Объем должен быть больше 0");
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
            return base.ToString() + $", Объем {Volume} м^3";
        }
    }
}
