namespace Mini_GPT.Models
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string ChatCollection { get; set; }
    }
}