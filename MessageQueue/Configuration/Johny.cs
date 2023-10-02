using Controlleur;

namespace Configuration;

public class Johny : IJohny
{
    public string GetMessage()
    {
        return "Hello World!";
    }
}