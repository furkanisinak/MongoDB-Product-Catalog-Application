using Program.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Program.Utils;


namespace Program.Services
{
    public class CategoryService
    {

        private readonly IMongoCollection<Category> _categoryCollection;


        public CategoryService()
        {
            DBMongo dbMongo = new();
            _categoryCollection = dbMongo.GetCollection<Category>("category");
        }


        public int AddCategory(Category category) // Kategori ekleme
        {
            try
            {
                _categoryCollection.InsertOne(category);
                return 1;
            }
            catch (MongoWriteException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }

        public bool UpdateCategory(Category category) // Kategori Güncelleme
        {
            var filter = Builders<Category>.Filter.Eq(item => item.CId, category.CId);
            ReplaceOneResult replaceOneResult = _categoryCollection.ReplaceOne(filter, category);
            return replaceOneResult.ModifiedCount > 0;
        }

        // İki parametreli güncelleme metodu eklendi
        public bool UpdateCategory(int cId, string newName)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.CId, cId);
            var update = Builders<Category>.Update.Set(x => x.CName, newName);
            var result = _categoryCollection.UpdateOne(filter, update);
            return result.ModifiedCount > 0;
        }

        // Kategori Listeleme
        public List<Category> GetAllCategories()
        {
            return _categoryCollection.Find(_ => true).ToList();
        }

        // Kategori id'sine göre silme
        public void DeleteCategory(string ID)
        {
            _categoryCollection.DeleteOne(x => x.Id.ToString() == ID);
        }

        // Kategori id'sine göre getir.
        public Category GetCategoryById(string id)
        {
            var filter = Builders<Category>.Filter.Eq(x => x.Id, new ObjectId(id));
            return _categoryCollection.Find(filter).FirstOrDefault();
        }



    } 
}