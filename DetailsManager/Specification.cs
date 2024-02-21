using System.Text.Json;

namespace DetailsManager;

public class Specification
{
    public event EventHandler<UpdateArgs> Updated;
    public string SpecName { get; }
    public double SpecPrice { get; }
    public bool IsCustom { get; }

    public Specification(string specName, double specPrice, bool isCustom)
    {
        SpecName = specName;
        SpecPrice = specPrice;
        IsCustom = isCustom;
    }

    public string ToJSON() => JsonSerializer.Serialize(this);
    
    protected virtual void OnUpdated(UpdateArgs e)
    {
        Updated.Invoke(this, e);
    }
}