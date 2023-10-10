namespace Controlleur.Classe
{
    public class MessageLog721
    {
        public MessageLog721(string message) {
            this.id = new Guid();
            this.message = message;
            this.date_time = DateTime.Now;
        }
        public Guid id
        {
            get;
            set;
        }

        public string message
        {
            get;
            set;
        }
        public DateTime date_time
        {
            get;
            set;
        }
    }
}