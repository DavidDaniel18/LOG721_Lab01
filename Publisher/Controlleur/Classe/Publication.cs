using System.Xml;
using System.Text.Json;
using Controlleur.Interface;

namespace Controlleur.Classe
{
    class Publication
    {
        private Topic topic;
        private Dictionary<string, string> content;

        public static JsonDocument fromCanonicaltoJSON()
        {
            throw new NotImplementedException();
        }

        public static XmlDocument fromCanonicaltoXML()
        {
            throw new NotImplementedException();
        }

        public static Message fromJSONtoCanonical(JsonDocument json)
        {
            JsonElement root = json.RootElement;
            string str_message;

            if (root.TryGetProperty("message", out JsonElement json_message))
            {
                str_message = json_message.GetString();
            }
            else
            {
                throw new InvalidOperationException("Invalid JSON. Vous devez spécifier une valeur pour la clé \"message\"");
            }


            Message message = new Message(new Guid(), str_message, new DateTime());
            return message;
        }

        public static Message fromXMLtoCanonical(XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }
    }
}