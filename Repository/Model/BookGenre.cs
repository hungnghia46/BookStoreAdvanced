using MongoDB.Bson.Serialization.Attributes;


namespace Repository.Model
{
    public class BookGenre
    {
        [BsonId]
        public String Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
    }
}
