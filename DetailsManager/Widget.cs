using System.Text.Json;

namespace DetailsManager;

public class Widget
{
    private const string DtfFormat = "YYYY-MM-DDThh:mm:ss.mls";
    private readonly string _widgetId;
    private string _name;
    private int _quantity;
    private double _price;
    private bool _isAvailable;
    private string _manufactureDate;
    private Specification[] _specifications;

    public string WidgetId => _widgetId;
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

    public double Price
    {
        get => _price;
    }

    public bool IsAvailable
    {
        get => _isAvailable;
        set
        {
            _isAvailable = value;
            OnUpdated();
        }
    }

    public string ManufactureDate
    {
        get => _manufactureDate;
        set
        {
            if (value.Length < DtfFormat.Length - 4) throw new ArgumentException($"Too short to meet the format: {DtfFormat}");
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
            if (value.Length > DtfFormat.Length - 4)
            {
                var ok = value[18] == '.';
                for (var i = 19; i < value.Length; ++i)
                {
                    if (char.IsDigit(value[i])) continue;
                    ok = false;
                    break;
                }
                if (!ok) throw new ArgumentException($"<Milliseconds> value doesn't meet the format {DtfFormat}");
            }

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

    public Specification[] Specifications
    {
        get => _specifications;
        set
        {
            if (value.Length == 0) throw new ArgumentException("An array of specifications can't be empty");
            _specifications = value;
            foreach (var u in _specifications)
            {
                u.Updated += PriceUpdate;
            }
            OnUpdated();
        }
    }
    
    public event EventHandler<EventArgs>? Updated;
    
    public string ToJSON() => JsonSerializer.Serialize(this);
    
    public Widget(string widgetId, string name, int quantity, double price, bool isAvailable, string manufactureDate, Specification[] specifications)
    {
        _widgetId = widgetId;
        _name = name;
        _quantity = quantity;
        _price = price;
        _isAvailable = isAvailable;
        _manufactureDate = manufactureDate;
        _specifications = specifications;
    }

    void PriceUpdate(object? sender, UpdateArgs e)
    {
        _price += e.Delta ?? 0;
    }
    protected virtual void OnUpdated()
    {
        var e = new UpdateArgs
        {
            TimeReached = DateTime.Now
        };
        Updated?.Invoke(this, e);
    }
    
}