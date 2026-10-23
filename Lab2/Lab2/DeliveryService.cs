using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class DeliveryService
    {
        private List<Shipment> _shipments;
        public DeliveryService()
        {
            _shipments = new List<Shipment>();
        }
        public void AddShipment(Shipment shipment)
        {
            if (shipment != null)
                _shipments.Add(shipment);
        }
        public List<Shipment> GetAll()
        {
            return _shipments;
        }
        public void PrintStatistics()
        {
            Console.WriteLine("\n статистика по типам");
            var groups = _shipments.GroupBy(s => s.GetType().Name);
            foreach ( var group in groups)
            {
                decimal totalCost = group.Sum(s => s.CalculateCost());
                Console.WriteLine($"{group.Key}: {group.Count()} шт. на сумму {totalCost:F2} руб.");
            }
        }
    }
}
