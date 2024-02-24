namespace DetailsManager.Protocols;

/// <summary>
/// The objects that confront this interface can be displayed in the View component.
/// </summary>
public interface IDisplayable
{
    /// <summary>
    /// Must return the interactive view components of all the required properties.
    /// </summary>
    /// <returns></returns>
    public List<IOption> GetOptions();

    /// <summary>
    /// Attempts to set an option to the object.
    /// </summary>
    /// <param name="val"></param>
    public void SetOption(IOption val);
}