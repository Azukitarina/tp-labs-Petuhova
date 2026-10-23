namespace Lab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DeliveryService service = new DeliveryService();
            service.AddShipment(new Letter("L-001", 0.1, false));
            service.AddShipment(new Letter("L-002", 0.2, true));
            service.AddShipment(new Parcel("P-001", 5.5, false));
            service.AddShipment(new OversizedCargo("O-001", 150.0, 2.5, false));
            service.AddShipment(new OversizedCargo("O-002", 500.0, 10.0, true));
            Console.WriteLine("Список всех отправлений");
            foreach (Shipment s in service.GetAll())
            {
                Console.WriteLine($"{s.ToString()} | Стоимость: {s.CalculateCost():F2} руб.");
            }
            Console.ReadLine();
        }
    }
}
