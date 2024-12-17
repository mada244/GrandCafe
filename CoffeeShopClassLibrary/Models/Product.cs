using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CoffeeShopClassLibrary.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [BsonElement("Denumire")]
        [JsonPropertyName("Denumire")]
        public string Denumire { get; set; }

        [BsonElement("Pret")]
        [JsonPropertyName("Pret")]
        public string Pret { get; set; }

        [BsonElement("Descriere")]
        [JsonPropertyName("Descriere")]
        public string Descriere { get; set; }

        [BsonElement("Category")]
        [JsonPropertyName("Category")]
        public string Category { get; set; }

        [BsonElement("ImageUrl")]
        [JsonPropertyName("ImageUrl")]
        public string ImageUrl { get; set; }
    }
}
