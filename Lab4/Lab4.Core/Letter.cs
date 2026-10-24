using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Core
{
    public class Letter : Shipment
    {
        public Letter(string trackingNumber, double weight, bool isUrgent)
            : base(trackingNumber, weight, isUrgent)
        {

        }
        public override decimal CalculateCost()
        {
            decimal baseCost = 100m + (decimal)Weight * 50m;
            if (IsUrgent)
                baseCost *= 1.5m;
            return baseCost;
        }

    }
}
