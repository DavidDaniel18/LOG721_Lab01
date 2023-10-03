using System.Xml;
using System.Text.Json;
using Controlleur.Interface;

namespace Controlleur.Classe
{
    class Publication : IPublication
    {
        private Topic topic;
        private Dictionary<string, string> content;

        public JsonDocument fromCanonicaltoJSON()
        {
            throw new NotImplementedException();
        }

        public XmlDocument fromCanonicaltoXML()
        {
            throw new NotImplementedException();
        }

        public void fromJSONtoCanonical(JsonDocument json)
        {
            throw new NotImplementedException();
        }

        public void fromXMLtoCanonical(XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }
    }
}