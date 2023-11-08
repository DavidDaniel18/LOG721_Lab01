namespace Data
{
    public class Group
    {
        public int groupId { get; set; }
        public Data data { get; set; }

        public Group(int groupId, Data data)
        {
            this.groupId = groupId;
            this.data = data;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Group;

            if (item == null)
            {
                return false;
            }

            return this.groupId == item.groupId && this.data.Equals(item.data);
        }

    }
}
