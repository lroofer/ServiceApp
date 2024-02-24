namespace DetailsManager;

public interface IOption
{
    public bool IsMutable();
    
    public string GetTag();
    
    public void Expand(IDisplayable @object);
}