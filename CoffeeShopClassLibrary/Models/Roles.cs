using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopClassLibrary.Models
{
    public class Roles
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }

        [BsonElement("roleName")]
        public string roleName { get; set; }

        [BsonElement("permissions")]
        public string permissions { get; set; }

    }
}