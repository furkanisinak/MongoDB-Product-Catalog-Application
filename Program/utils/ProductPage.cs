using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using Program.Models;

namespace Program.Utils
{
    public class ProductPage
    {
        public int TotalCount { get; set; }// sayfa boyutu
        public int TotalPage { get; set; } // toplam sayfa sayısı
        public List<Product>? Products { get; set; } // kullanıcı listesi
    }

}    