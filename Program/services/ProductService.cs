using Program.Models;
using MongoDB.Driver;
using Program.Utils;
using MongoDB.Bson;


namespace Program.Services
{
    public class ProductService
    {

        private readonly IMongoCollection<Product> _productCollection;


        public ProductService()
        {
            DBMongo dbMongo = new();
            _productCollection = dbMongo.GetCollection<Product>("product");
        }

        // Ürün listeleme
        public ProductPage GetAllProductsPage(int pageSize = 10, int pageNumber = 1)
        {
            var filter = Builders<Product>.Filter.Empty; // tüm ürünleri al
            var totalCount = _productCollection.CountDocuments(filter); // toplam ürün sayısını al
            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
            var products = _productCollection.Find(filter)
                .SortByDescending(x => x.saveDate) // en son eklenen ürünleri al
                .Skip(pageSize * (pageNumber - 1)) // sayfa boyutuna göre atla
                .Limit(pageSize) // sayfa boyutuna göre sınırla
                .ToList(); // listeyi al

            ProductPage productPage = new()
            {
                TotalCount = (int)totalCount,
                TotalPage = totalPage,
                Products = products
            };
            return productPage;

        }

        // Ürün Ekleme
        public int AddProduct(Product product)
        {
            try
            {
                _productCollection.InsertOne(product);
                return 1;
            }
            catch (MongoWriteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        // Ürün Güncelleme
        public bool UpdateProduct(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, product.Id);
            var replaceOneResult = _productCollection.ReplaceOne(filter, product);
            return replaceOneResult.ModifiedCount > 0;
        }

        // Ürün ID'sine göre sil
        public void DeleteProductById(string Id)
        {
            _productCollection.DeleteOne(x => x.Id.ToString() == Id);
        }

        // Ürün ID'sine göre getir.
        public Product GetProductById(string Id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, new ObjectId(Id));
            return _productCollection.Find(filter).FirstOrDefault();
        }

        // Ürün adında veya açıklamasında belirtilen anahtar kelimeyi içeren ürünleri getir
        public List<Product> SearchProducts(string anahtarKelime)
        {
            var filter = Builders<Product>.Filter.Or(
                Builders<Product>.Filter.Regex(x => x.UrunAdı, new BsonRegularExpression(anahtarKelime, "i")),
                Builders<Product>.Filter.Regex(x => x.Description, new BsonRegularExpression(anahtarKelime, "i"))
            );
            return _productCollection.Find(filter).ToList();
        }

        // Ürünleri kategoriye göre filtrele
        public List<Product> GetProductsByCategoryId(int categoryId)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.CategoryId, categoryId);
            return _productCollection.Find(filter).ToList();
        }

    }
}