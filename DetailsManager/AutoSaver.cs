using System.Diagnostics;
using System.Text.Json;
using DetailsManager.Arguments;
using DetailsManager.Objects;

namespace DetailsManager;

/// <summary>
/// Auto-saving daemon, that write every 15 seconds data to the temporary file.
/// </summary>
public class AutoSaver
{
    private DateTime _lastApplied;

    /// <summary>
    /// The method asynchronously updates the temporary file.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnUpdate(object? sender, UpdateArgs e)
    {
        var diff = (e.TimeReached - _lastApplied).TotalSeconds;
        if (!(diff >= 15)) return;
        try
        {
            await using var createStream = File.Create(Manager.TempFileName ?? "data_tmp.json");
            await JsonSerializer.SerializeAsync(createStream, Manager.Widgets);
            _lastApplied = e.TimeReached;
        }
        catch (Exception exception)
        {
            Debug.WriteLine($"Unable to write a file {exception.Message}");
        }
    }

    public void WriteFile(string path)
    {
        var jsonString = JsonSerializer.Serialize(Manager.Widgets);
        File.WriteAllText(path, jsonString);
    }
    
    public AutoSaver()
    {
        _lastApplied = DateTime.Now;
        Widget.Updated += OnUpdate;
        Specification.Updated += OnUpdate;
    }
}