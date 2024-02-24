namespace DetailsManager.Protocols;

/// <summary>
/// Interactive options.
/// </summary>
public interface IOption
{
    /// <summary>
    /// Can the option value be changed?
    /// </summary>
    /// <returns>true if can</returns>
    public bool IsMutable();

    public string GetTag();

    /// <summary>
    /// Creates an additional view of the given object.
    /// </summary>
    /// <param name="object">Displayable object.</param>
    public void Expand(IDisplayable @object);
}