public abstract class Shipment
{
    private readonly string _number;
    private readonly double _weight;
    private readonly bool _urgent;

    public string Number => _number;
    public double Weight => _weight;
    public bool Urgent => _urgent;

    protected Shipment(string number, double weight, bool urgent)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Номер не может быть пустым.", nameof(number));
        if (weight <= 0)
            throw new ArgumentOutOfRangeException(nameof(weight), "Вес должен быть > 0.");
        _number = number;
        _weight = weight;
        _urgent = urgent;
    }

    public abstract double BaseCost();
    public virtual double TotalCost()
    {
        double cost = BaseCost();
        if (_urgent) cost *= 1.5;
        return cost;
    }
    public override string ToString()
        => $"{GetType().Name} №{_number}, вес {_weight:F2} кг" + (_urgent ? " (срочно)" : "") + $", стоимость {TotalCost():F2} руб.";
    public int CompareTo(Shipment? other)
    {
        if (other is null) return 1;
        return TotalCost().CompareTo(other.TotalCost());
    }
}

public class Letter : Shipment
{
    private readonly bool _registered;
    public bool Registered => _registered;
    public Letter(string number, double weight, bool urgent, bool registered)
        : base(number, weight, urgent)
    {
        _registered = registered;
    }
    public override double BaseCost() => _registered ? 70.0 : 50.0;
    public override string ToString()
        => base.ToString() + (_registered ? ", заказное" : ", простое");
}

public class Parcel : Shipment
{
    private readonly double _length, _width, _height;
    public double Length => _length;
    public double Width => _width;
    public double Height => _height;
    public double Volume => _length * _width * _height;

    public Parcel(string number, double weight, bool urgent,
                  double length, double width, double height)
        : base(number, weight, urgent)
    {
        if (length <= 0 || width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Габариты должны быть > 0.");
        _length = length;
        _width = width;
        _height = height;
    }
    public override double BaseCost() => 100.0 + 30.0 * Weight;
}

public class Oversized : Shipment
{
    private readonly double _length, _width, _height;

    public double Length => _length;
    public double Width => _width;
    public double Height => _height;
    public double Volume => _length * _width * _height;
    public Oversized(string number, double weight, bool urgent,
                     double length, double width, double height)
        : base(number, weight, urgent)
    {
        if (length <= 0 || width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Габариты должны быть > 0.");
        _length = length;
        _width = width;
        _height = height;
    }
    public override double BaseCost() => 200.0 + 500.0 * Volume;
    public override double TotalCost()
    {
        double cost = BaseCost();
        if (Urgent) cost *= 2.0;
        return cost;
    }
    public override string ToString()
        => base.ToString() + $", объем {Volume:F3} м^3";
}

public class DeliveryService
{
    private readonly List<Shipment> _shipments = new();
    public void Add(Shipment s)
    {
        if (s == null) throw new ArgumentNullException(nameof(s));
        _shipments.Add(s);
    }
    public IReadOnlyList<Shipment> All => _shipments;
    public double TotalRevenue() => _shipments.Sum(s => s.TotalCost());
    public int UrgentCount() => _shipments.Count(s => s.Urgent);
    public Shipment? MostExpensive()
        => _shipments.OrderByDescending(s => s.TotalCost()).FirstOrDefault();
    public IEnumerable<IGrouping<string, Shipment>> ByType()
        => _shipments.GroupBy(s => s.GetType().Name);
}

class Program                                            
{
    static void Main()
    {
        var service = new DeliveryService();

        service.Add(new Letter("L-001", 0.02, urgent: false, registered: false));
        service.Add(new Letter("L-002", 0.03, urgent: true, registered: true));
        service.Add(new Parcel("P-101", 2.5, urgent: false, 30, 20, 15));
        service.Add(new Parcel("P-102", 5.0, urgent: true, 40, 30, 20));
        service.Add(new Oversized("O-201", 40.0, urgent: false, 1.5, 1.0, 0.8));
        service.Add(new Oversized("O-202", 80.0, urgent: true, 2.0, 1.5, 1.0));

        Console.WriteLine("Все отправления");
        foreach (var s in service.All)
            Console.WriteLine(s);

        Console.WriteLine();
        Console.WriteLine($"Всего отправлений:    {service.All.Count}");
        Console.WriteLine($"Срочных:              {service.UrgentCount()}");
        Console.WriteLine($"Суммарная выручка:    {service.TotalRevenue():F2} руб.");

        var top = service.MostExpensive();
        Console.WriteLine($"Самое дорогое:        {top}");

        Console.WriteLine();
        Console.WriteLine("По типам");
        foreach (var group in service.ByType())
        {
            Console.WriteLine($"{group.Key}: {group.Count()} шт., " +
                              $"выручка {group.Sum(s => s.TotalCost()):F2} руб.");
        }
    }
}
