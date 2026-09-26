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
