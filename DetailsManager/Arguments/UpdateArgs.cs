namespace DetailsManager.Arguments;

public class UpdateArgs : EventArgs
{
    public DateTime TimeReached { get; init; }

    public UpdateArgs()
    {
        TimeReached = DateTime.Now;
    }
}