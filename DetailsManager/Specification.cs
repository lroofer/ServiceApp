using System.Text.Json;
using System.Text.Json.Serialization;

namespace DetailsManager;

public class Specification: IDisplayable
{
    private string _specName;
    private double _specPrice;
    private bool _isCustom;
    public event EventHandler<PriceUpdateArgs>? PriceUpdated;
    public static event EventHandler<UpdateArgs>? Updated; 
    [JsonPropertyName("specName")]
    public string SpecName
    {
        get => _specName;
        set
        {
            if (value.Length < 3)
                throw new ArgumentException("Spec name must contain more than 3 symbols");
            _specName = value;
            OnUpdated();
        }
    }

    [JsonPropertyName("specPrice")]
    public double SpecPrice
    {
        get => _specPrice;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price can't be negative");
            var delta = -_specPrice + value;
            _specPrice = value;
            OnPriceUpdated(delta);
            OnUpdated();
        }
    }

    [JsonPropertyName("isCustom")]
    public bool IsCustom
    {
        get => _isCustom;
        set
        {
            _isCustom = value;
            OnUpdated();
        }
    }

    public Specification(string specName, double specPrice, bool isCustom)
    {
        _specName = specName;
        _specPrice = specPrice;
        _isCustom = isCustom;
    }

    public string ToJson() => JsonSerializer.Serialize(this);
    
    protected virtual void OnPriceUpdated(double delta)
    {
        var e = new PriceUpdateArgs
        {
            Delta = delta
        };
        PriceUpdated?.Invoke(this, e);
    }

    protected virtual void OnUpdated()
    {
        var e = new UpdateArgs
        {
            TimeReached = DateTime.Now
        };
        Updated?.Invoke(this, e);
    }

    public List<IOption> GetOptions()
        => new()
        {
            new StringElement("specName", SpecName, true),
            new DoubleElement("specPrice", SpecPrice, true),
            new BoolElement("isCustom", IsCustom)
        };

    public void SetOption(IOption val)
    {
        switch (val.GetTag())
        {
            case "specName":
                SpecName = (val as StringElement)?.Value ?? throw new InvalidOperationException();
                break;
            case "specPrice":
                SpecPrice = (val as DoubleElement)?.Value ?? throw new InvalidOperationException();
                break;
            case "isCustom":
                IsCustom = (val as BoolElement)?.Value ?? throw new InvalidOperationException();
                break;
            default:
                throw new ArgumentException("The object wasn't found");
        }
    }
}