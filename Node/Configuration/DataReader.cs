using System.Resources;
using Infrastructure.FileHandlers.Interfaces;

namespace Node;

public class DataReader : IDataReader
{
    private readonly ResourceManager _manager;

    public DataReader()
    {
        _manager = new ResourceManager("Configuration.Properties.Resources", typeof(Program).Assembly);
    }

    public string GetString(string key)
    {
        var data = _manager.GetString(key);

        if (string.IsNullOrWhiteSpace(data)) throw new Exception($"Data not found for key {key}");

        return data;
    }

    public object GetObject(string key)
    {
        var data = _manager.GetObject(key);

        if (data is null) throw new Exception($"Data not found for key {key}");

        return data;
    }
}