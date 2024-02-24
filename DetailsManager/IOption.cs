namespace DetailsManager;

public interface IOption
{
    public bool IsMutable();

    public void SetValue(string value);
    
    public string GetTag();
    
    public void Expand();
}