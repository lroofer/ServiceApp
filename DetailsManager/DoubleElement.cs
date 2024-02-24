namespace DetailsManager;

public class DoubleElement: IOption
{
    private bool _isMutable;
    private string _tag;

    public DoubleElement(string tag, double value, bool isMutable)
    {
        _tag = tag;
        _isMutable = isMutable;
        GetValue = value;
    }

    public double GetValue { get; private set; }
        
    public bool IsExpandable() => false;

    public bool IsMutable() => _isMutable;

    public void SetValue(string value)
    {
        if (!double.TryParse(value, out var nVal))
        {
            throw new ArgumentException("Value must be double.");
        }

        GetValue = nVal;
    }

    public string GetTag()
        => _tag;

    public void Expand()
        => throw new InvalidOperationException();
    
    public override string ToString() => $"{_tag}: {Math.Round(GetValue, 2)}";

}