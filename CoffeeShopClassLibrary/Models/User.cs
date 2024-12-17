using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CoffeeShopClassLibrary.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [BsonElement("username")]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [BsonElement("email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [BsonElement("createdAt")]
        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("roles")]
        [JsonPropertyName("roles")]
        public string Roles { get; set; }

        [BsonElement("isActive")]
        [JsonPropertyName("isActive")]
        public string IsActive { get; set; }

        [BsonElement("favouriteProducts")]
        [JsonPropertyName("favouriteProducts")]
        public List<string> FavouriteProducts { get; set; }
    }
}
