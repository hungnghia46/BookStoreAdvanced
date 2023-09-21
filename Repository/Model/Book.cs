using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Model
{
    public class Book
    {
        private DateTime _createdAtDate;
        private DateTime _updatedAtDate;

        [BsonId]
        // [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("genre")]
        public string Genre { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("publicationYear")]
        public int PublicationYear { get; set; }

        [BsonElement("isbn")]
        public string ISBN { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("inventoryQuantity")]
        public int InventoryQuantity { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt
        {
            get
            {
                // Specify the time zone you want to convert to (GMT+7)
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // This is the ID for GMT+7

                // Convert the existing DateTime (_date) to the target time zone (GMT+7)
                DateTime gmtPlus7Time = TimeZoneInfo.ConvertTime(_createdAtDate, targetTimeZone);

                return gmtPlus7Time;
            }
            set
            {
                _createdAtDate = value;
            }
        }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt
        {
            get
            {
                TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // This is the ID for GMT+7

                DateTime gmtPlus7Time = TimeZoneInfo.ConvertTime(_updatedAtDate, targetTimeZone);

                return gmtPlus7Time;
            }
            set
            {
                _updatedAtDate = value;
            }
        }
    }
}
