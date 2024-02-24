using System.Text.Json;
using System.Text.Json.Serialization;
using DetailsManager.Arguments;
using DetailsManager.Protocols;
using DetailsManager.Views.SubElements;

namespace DetailsManager.Objects;

public class Widget : IDisplayable
{
    private const string DtfFormat = "YYYY-MM-DDThh:mm:ss.mls";

    public static readonly string[] WidgetProperties =
        { "widgetId", "name", "quantity", "price", "isAvailable", "manufactureDate" };

    private readonly List<Specification> _specifications;
    private string _name;
    private string _manufactureDate;
    private int _quantity;
    private double _price;
    private bool _isAvailable;

    public static event EventHandler<UpdateArgs>? Updated;

    public Widget(string widgetId = "", string name = "", int quantity = 0, double price = 0, bool isAvailable = false, string manufactureDate = "",
        List<Specification> specifications = null!)
    {
        WidgetId = widgetId;
        _name = name;
        _quantity = quantity;
        _price = price;
        _isAvailable = isAvailable;
        _manufactureDate = manufactureDate;
        _specifications = specifications;
        foreach (var u in _specifications)
        {
            u.PriceUpdated += PriceUpdate;
        }
    }

    /// <summary>
    /// Creates a snapshot of the current object state.
    /// </summary>
    /// <returns>Viewable data.</returns>
    public List<IOption> GetOptions()
        => new()
        {
            new StringElement("widgetId", WidgetId, false),
            new StringElement("name", Name, true),
            new IntElement("quantity", Quantity),
            new DoubleElement("price", Price, false),
            new BoolElement("isAvailable", IsAvailable),
            new StringElement("manufactureDate", ManufactureDate, true),
            new ArrayElement("specifications", in _specifications)
        };

    /// <summary>
    /// Attempts to apply changes in the view to the object.
    /// </summary>
    /// <param name="val">Viewable object.</param>
    /// <exception cref="InvalidOperationException">Tag doesn't correspond the object.</exception>
    /// <exception cref="ArgumentException">Tag doesn't exist.</exception>
    public void SetOption(IOption val)
    {
        switch (val.GetTag())
        {
            case "name":
                Name = (val as StringElement)?.Value ?? throw new InvalidOperationException();
                break;
            case "quantity":
                Quantity = (val as IntElement)?.Value ?? throw new InvalidOperationException();
                break;
            case "isAvailable":
                IsAvailable = (val as BoolElement)?.Value ?? throw new InvalidOperationException();
                break;
            case "manufactureDate":
                ManufactureDate = (val as StringElement)?.Value ?? throw new InvalidOperationException();
                break;
            default:
                throw new ArgumentException("The object wasn't found");
        }
    }

    [JsonPropertyName("widgetId")] public string WidgetId { get; }

    [JsonPropertyName("name")]
    public string Name
    {
        get => _name;
        set
        {
            if (value.Length >= 3)
            {
                _name = value;
                OnUpdated();
            }
            else throw new ArgumentException("The name must contain at least than 3 symbols.");
        }
    }

    [JsonPropertyName("quantity")]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value >= 0)
            {
                _quantity = value;
                OnUpdated();
            }
            else throw new ArgumentException("Quantity can't be negative.");
        }
    }

    [JsonPropertyName("price")] public double Price => _price;

    [JsonPropertyName("isAvailable")]
    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            _isAvailable = value;
            OnUpdated();
        }
    }

    [JsonPropertyName("manufactureDate")]
    public string ManufactureDate
    {
        get => _manufactureDate;
        set
        {
            if (value.Length < DtfFormat.Length - 4)
                throw new ArgumentException($"Too short to meet the format: {DtfFormat}");
            if (!int.TryParse("" + value[0] + value[1] + value[2] + value[3], out int year))
                throw new ArgumentException($"<Year> value doesn't meet the format {DtfFormat}");
            if (!int.TryParse("" + value[5] + value[6], out int month))
                throw new ArgumentException($"<Month> value doesn't meet the format {DtfFormat}");
            if (!int.TryParse("" + value[8] + value[9], out int day))
                throw new ArgumentException($"<Day> value doesn't meet the format {DtfFormat}");
            if (!int.TryParse("" + value[11] + value[12], out int hour))
                throw new ArgumentException($"<Hours> value doesn't meet the format {DtfFormat}");
            if (!int.TryParse("" + value[14] + value[15], out int minute))
                throw new ArgumentException($"<Minutes> value doesn't meet the format {DtfFormat}");
            if (!int.TryParse("" + value[17] + value[18], out int seconds))
                throw new ArgumentException($"<Seconds> value doesn't meet the format {DtfFormat}");
            if ("" + value[4] + value[7] + value[10] + value[13] + value[16] != "--T::")
                throw new ArgumentException($"Separators don't meet the format {DtfFormat}");
            try
            {
                _ = new DateTime(year, month, day, hour, minute, seconds);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException("Given date/time doesn't exist");
            }

            _manufactureDate = value;
            OnUpdated();
        }
    }

    [JsonPropertyName("specifications")] public List<Specification> Specifications => _specifications;

    /// <summary>
    /// Creates a snippet of the object.
    /// </summary>
    /// <returns>Presented info</returns>
    public override string ToString() => $"{Name} ({Quantity}) -> {Math.Round(Price, 2)}$: {WidgetId}";

    /// <summary>
    /// Converts specification object to JSON-formatted string.
    /// </summary>
    /// <returns>JSON-formatted representation of the object.</returns>
    public string ToJson() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Manually add a new specification to the list.
    /// </summary>
    /// <param name="specification">New specification to add.</param>
    public void AddSpecification(Specification specification)
    {
        _specifications.Add(specification);
        _price += specification.SpecPrice;
        specification.PriceUpdated += PriceUpdate;
    }

    /// <summary>
    /// Manually remove specification from the list.
    /// </summary>
    /// <param name="index">Index of the element.</param>
    /// <exception cref="ArgumentOutOfRangeException">Index was out of range.</exception>
    public void RemoveSpecification(int index)
    {
        if (index >= _specifications.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        _price -= _specifications[index].SpecPrice;
        _specifications[index].PriceUpdated -= PriceUpdate;
        _specifications.RemoveAt(index);
    }

    /// <summary>
    /// Invokes all the subscribers when something has been changed.
    /// </summary>
    protected virtual void OnUpdated()
    {
        var e = new UpdateArgs
        {
            TimeReached = DateTime.Now
        };
        Updated?.Invoke(this, e);
    }

    /// <summary>
    /// Private method changes the price value after getting new event notification.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">Parameters of the change.</param>
    private void PriceUpdate(object? sender, PriceUpdateArgs e)
    {
        _price += e.Delta ?? 0;
        OnUpdated();
    }
}