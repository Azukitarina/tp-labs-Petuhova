using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Core
{
    public class Parcel : Shipment
    {
        public Parcel(string trackingNumber, double weight, bool isUrgent)
            : base(trackingNumber, weight, isUrgent)
        {
        }
        public override decimal CalculateCost()
        {
            decimal baseCost = 300m + (decimal)Weight * 100m;
            if (IsUrgent)
                baseCost *= 2.0m;
            return baseCost;
        }
    }
}
