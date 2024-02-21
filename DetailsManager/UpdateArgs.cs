namespace DetailsManager;

public class UpdateArgs: EventArgs
{
    public DateTime TimeReached { get; set; }
    public int? Delta { get; set; }
}