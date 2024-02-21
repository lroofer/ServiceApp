namespace DetailsManager;

public class PriceUpdateArgs: EventArgs
{
    public double? Delta { get; set; }
}