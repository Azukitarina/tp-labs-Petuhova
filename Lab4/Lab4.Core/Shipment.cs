using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Core
{
    public abstract class Shipment : IComparable<Shipment>
    {

        public string TrackingNumber { get; }
        public double Weight { get; }
        public bool IsUrgent { get; }

        protected Shipment(string trackingNumber, double weight, bool isUrgent)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
                throw new ArgumentException("Номер не может быть пустым", nameof(trackingNumber));
            if (weight <= 0)
                throw new ArgumentOutOfRangeException(nameof(weight), "Вес должен быть больше 0");
            TrackingNumber = trackingNumber;
            Weight = weight;
            IsUrgent = isUrgent;
        }
        public int CompareTo(Shipment? other)
        {
            if (other == null) return 1;
            return CalculateCost().CompareTo(other.CalculateCost());
        }
        public abstract decimal CalculateCost();
        public override string ToString()
        {
            string urgency = IsUrgent ? "Срочно" : "Обычно";
            return $"{GetType().Name}: Трек {TrackingNumber}, Вес {Weight} кг ({urgency})";
        }
    }
}
