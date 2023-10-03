using Controlleur.Interface;

namespace Controlleur.Classe
{
    class Topic : ITopic
    {
        // Clé de routage du topic Ex: topic1.topic2.topic3
        private string name;
        // J'imagine qu'il faut avoir une classe Client ?
        private List<dynamic> pub;
        private List<dynamic> sub;
        public string getName()
        {
            return name;
        }

        public List<dynamic> getPub()
        {
            return pub;
        }

        public List<dynamic> getSub()
        {
            return sub;
        }
    }
}