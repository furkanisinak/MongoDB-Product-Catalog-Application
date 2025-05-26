using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Program.Models
{
    public class Product
    {
        public ObjectId Id { get; set; }

        [BsonElement("id")]
        public int UId { get; set; }

        [BsonElement("ad")]
        public string UrunAdı { get; set; } = null!;

        [BsonElement("aciklama")]
        public string Description { get; set; } = null!;

        [BsonElement("fiyat")]
        public double Price { get; set; } 

        [BsonElement("stokAdeti")]
        public int Stock { get; set; }

        [BsonElement("kategoriId")]
        public int CategoryId { get; set; }

        [BsonElement("eklenmeTarihi")]
        public DateTime saveDate { get; set; }
    }
}