namespace DetailsManager;

public class BoolElement: IOption
{
    private bool _value;
    private string _tag;

    public BoolElement(string tag, bool value)
    {
        _tag = tag;
        _value = value;
    }

    public bool IsMutable()
        => true;

    public void SetValue(string value)
    {
        throw new NotImplementedException();
    }

    public string GetTag()
        => _tag;

    public override string ToString()
        => $"{_tag}: {(_value ? "V": "X")}";

    public void Expand()
    {
        throw new NotImplementedException();
    }
}