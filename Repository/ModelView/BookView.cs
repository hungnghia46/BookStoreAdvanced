using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ModelView
{
    public class BookView
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int PublicationYear { get; set; }
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public int InventoryQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
