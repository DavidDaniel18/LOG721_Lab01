namespace Infrastructure.FileHandlers.Interfaces;

public interface IDataReader
{
    string GetString(string key);

    object GetObject(string key);
}