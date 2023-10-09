using System.Xml;
using System.Text.Json;

namespace Controlleur.Interface
{
    interface IPublication
    {
        // Void � remplacer
        void fromXMLtoCanonical(XmlDocument xmlDocument);
        XmlDocument fromCanonicaltoXML();
        JsonDocument fromCanonicaltoJSON();
        // Void � remplacer
        void fromJSONtoCanonical(JsonDocument json);

    }
}