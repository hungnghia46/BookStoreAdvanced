using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Merchandise
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("description")]
    public string Description { get; set; }

    [BsonElement("category")]
    public string Category { get; set; }

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("inventoryQuantity")]
    public int InventoryQuantity { get; set; }

    [BsonElement("imageUrl")]
    public string ImageUrl { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}
