namespace Data
{
    public class Data
    {
        public int width { get; set; }
        public int price { get; set; }


        public Data(int width, int price)
        {
            this.width = width;
            this.price = price;
        }

        public double GetDistance(Data other)
        {
            return Math.Sqrt(Math.Pow(this.width - other.width, 2) + Math.Pow(this.price - other.price, 2));
        }


        public override bool Equals(object obj)
        {
            var item = obj as Data;

            if (item == null)
            {
                return false;
            }
            return this.width == item.width && this.price == item.price;
        }

    }
}