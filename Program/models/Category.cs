using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Program.Utils;
using Program.Services;
using Program.Models;

namespace Program.Models
{
    public class Category
    {
        
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public int CId { get; set; } 

        [BsonElement("categoryName")]
        public string CName { get; set; } = null!;
    }
}