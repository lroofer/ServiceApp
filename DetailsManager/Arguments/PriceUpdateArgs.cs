namespace DetailsManager.Arguments;

public class PriceUpdateArgs : EventArgs
{
    /// <summary>
    /// NewValue - OldValue.
    /// </summary>
    public double? Delta { get; set; }
}