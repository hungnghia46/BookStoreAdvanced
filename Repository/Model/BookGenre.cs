using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
