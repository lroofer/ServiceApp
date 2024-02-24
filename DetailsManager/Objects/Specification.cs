using System.Text.Json;
using System.Text.Json.Serialization;
using DetailsManager.Arguments;
using DetailsManager.Protocols;
using DetailsManager.Views.SubElements;

namespace DetailsManager.Objects;

public class Specification : IDisplayable
{
    private string _specName;
    private double _specPrice;
    private bool _isCustom;
    
    public Specification(string specName = "null", double specPrice = 0, bool isCustom = false)
    {
        _specName = specName;
        _specPrice = specPrice;
        _isCustom = isCustom;
    }

    public static event EventHandler<UpdateArgs>? Updated;
    public event EventHandler<PriceUpdateArgs>? PriceUpdated;

    /// <summary>
    /// Creates a snapshot of the current object state.
    /// </summary>
    /// <returns>Viewable data.</returns>
    public List<IOption> GetOptions()
        => new()
        {
            new StringElement("specName", SpecName, true),
            new DoubleElement("specPrice", SpecPrice, true),
            new BoolElement("isCustom", IsCustom)
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

    /// <summary>
    /// Converts specification object to JSON-formatted string.
    /// </summary>
    /// <returns>JSON-formatted representation of the object.</returns>
    public string ToJson() => JsonSerializer.Serialize(this);

    /// <summary>
    /// Creates a snippet of the object.
    /// </summary>
    /// <returns>Presented info</returns>
    public override string ToString()
        => $"{SpecName}: {Math.Round(SpecPrice, 2)}, isCustom: {(IsCustom ? "true" : "false")}";

    /// <summary>
    /// Invokes all the subscribers when the price has been updated.
    /// </summary>
    /// <param name="delta">New Value - Old Value</param>
    protected virtual void OnPriceUpdated(double delta)
    {
        var e = new PriceUpdateArgs
        {
            Delta = delta
        };
        PriceUpdated?.Invoke(this, e);
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
}