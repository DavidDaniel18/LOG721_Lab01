namespace Data
{
    public class Map
    {

        public static Dictionary<Group, List<Data>> createMaps(List<Group> groups, List<Data> datas)
        {
            Dictionary<Group, List<Data>> maps = new Dictionary<Group, List<Data>>();

            // Setter les maps
            foreach (Group group in groups)
            {
                maps[group] = new List<Data>();
            }

            // Trouver le meilleur groupe pour un data et l'ajouter dans la bonne map
            foreach (Data data in datas)
            {
                Group best_group =  null;
                double value = Double.MaxValue;
                foreach (Group group in groups)
                {
                    double distance = data.GetDistance(group.data);
                    if (distance < value) {
                        value = distance;
                        best_group = group;
                    }
                }

                maps[best_group].Add(data);
            }

            return maps;
        }
    }

}