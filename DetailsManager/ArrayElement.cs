using ServiceApp;

namespace DetailsManager;

public class ArrayElement: View<Specification>, IOption
{
    private readonly string _tag;
    
    public ArrayElement(string tag, in List<Specification> lst): base(lst, "Manage list of specifications")
    {
        _tag = tag;
    }

    public bool IsMutable()
        => false;

    public void SetValue(string value)
    {
        throw new NotImplementedException();
    }

    public string GetTag()
        => _tag;
    
    public void Expand(IDisplayable @object)
    {
        Init();
        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when SelectedOption != 0:
                    Up();
                    break;
                case ConsoleKey.DownArrow when SelectedOption != Options.Count - 1:
                    Down();
                    break;
                case ConsoleKey.Enter:
                    Console.ResetColor();
                    var specificationView = new ObjectView(Options[SelectedOption], "Specification");
                    specificationView.Run();
                    Init();
                    break;
            }
        } while (key != ConsoleKey.X);
        Console.ResetColor();
        Console.Clear();
    }
    
    public override string ToString() => $"{_tag}: {Options.Count} inst.";

}