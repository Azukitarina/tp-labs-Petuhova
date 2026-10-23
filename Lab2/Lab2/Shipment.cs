using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public abstract class Shipment
    {
        private string _trackingNumber;
        private double _weight;
        private bool _isUrgent;
        public string TrackingNumber { get; }
        public double Weight { get; }
        public bool IsUrgent {  get; }

        public Shipment(string trackingNumber, double weight, bool isUrgent)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
                throw new ArgumentException("Номер не может быть пустым", nameof(trackingNumber));
            if (weight <= 0)
                throw new ArgumentOutOfRangeException(nameof(weight), "Вес должен быть больше 0");
            TrackingNumber = trackingNumber;
            Weight = weight;
            IsUrgent = isUrgent;
        }
        public abstract decimal CalculateCost();
    }
}
