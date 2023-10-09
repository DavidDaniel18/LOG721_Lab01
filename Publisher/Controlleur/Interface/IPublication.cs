using System.Xml;
using System.Text.Json;

namespace Controlleur.Interface
{
    interface IPublication
    {
        // Void à remplacer
        void fromXMLtoCanonical(XmlDocument xmlDocument);
        XmlDocument fromCanonicaltoXML();
        JsonDocument fromCanonicaltoJSON();
        // Void à remplacer
        void fromJSONtoCanonical(JsonDocument json);

    }
}