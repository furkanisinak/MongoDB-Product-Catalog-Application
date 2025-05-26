using Program.Utils;
using Program.Models;
using Program.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq; // .Any() metodu için gerekli

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            LogControl logControl = new LogControl();

            Console.WriteLine("Yapmak istediğiniz işlemi seçin:");
            Console.WriteLine("1- Ürün Ekleme");
            Console.WriteLine("2- Ürün Güncelleme");
            Console.WriteLine("3- Ürün Silme (MongoDB)");
            Console.WriteLine("4- Ürün Listeleme");
            Console.WriteLine("5- Ürün Arama (Anahtar Kelime)");
            Console.WriteLine("6- Ürün Filtreleme (Kategoriye Göre)");
            Console.WriteLine("7- Kategori Ekleme");
            Console.WriteLine("8- Kategori Güncelleme (CId'ye Göre)");
            Console.WriteLine("9- Kategori Silme (MongoDB)");
            Console.WriteLine("10- Kategori Listeleme");
            Console.WriteLine("11- Hata Loglarını Görüntüleme");
            Console.WriteLine("12- Hata Loglarında Arama Yapma");

            Console.Write("Seçiminiz: ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // Ürün Ekleme
                    Console.WriteLine("\n--- Ürün Ekleme ---");
                    ProductService productServiceAdd = new ProductService();
                    try
                    {
                        Console.Write("Ürün ID (UId): ");
                        if (!int.TryParse(Console.ReadLine(), out int uId))
                        {
                            throw new FormatException("Geçersiz Ürün ID formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Ürün Adı: ");
                        string urunAdi = Console.ReadLine()!;
                        Console.Write("Ürün Açıklaması: ");
                        string description = Console.ReadLine()!;
                        Console.Write("Ürün Fiyatı: ");
                        if (!double.TryParse(Console.ReadLine(), out double price))
                        {
                            throw new FormatException("Geçersiz Ürün Fiyatı formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Ürün Stok: ");
                        if (!int.TryParse(Console.ReadLine(), out int stock))
                        {
                            throw new FormatException("Geçersiz Ürün Stok formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Kategori ID (CategoryId): ");
                        if (!int.TryParse(Console.ReadLine(), out int categoryId_Product))
                        {
                            throw new FormatException("Geçersiz Kategori ID formatı. Lütfen sayısal bir değer girin.");
                        }

                        Product p1 = new()
                        {
                            Id = ObjectId.GenerateNewId(),
                            UId = uId,
                            UrunAdı = urunAdi,
                            Description = description,
                            Price = price,
                            Stock = stock,
                            CategoryId = categoryId_Product,
                            saveDate = DateTime.Now
                        };
                        int p1Save = productServiceAdd.AddProduct(p1);
                        Console.WriteLine($"Kayıt Yapılan Sayı: {p1Save}");
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Ürün Ekleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Ekleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "2":
                    // Ürün Güncelleme
                    Console.WriteLine("\n--- Ürün Güncelleme ---");
                    ProductService productServiceUpdate = new ProductService();
                    try
                    {
                        Console.Write("Güncellenecek Ürünün MongoDB ID'si (24 haneli hex string örn: 60c72834d9a3b2b8e8f8a1a2): ");
                        string productIdToUpdate = Console.ReadLine()!;
                        if (!ObjectId.TryParse(productIdToUpdate, out ObjectId parsedId))
                        {
                            throw new FormatException("Geçersiz MongoDB ID formatı.");
                        }

                        Console.Write("Yeni Ürün ID (UId): ");
                        if (!int.TryParse(Console.ReadLine(), out int newUId))
                        {
                            throw new FormatException("Geçersiz Yeni Ürün ID formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Yeni Ürün Adı: ");
                        string newUrunAdi = Console.ReadLine()!;
                        Console.Write("Yeni Ürün Açıklaması: ");
                        string newDescription = Console.ReadLine()!;
                        Console.Write("Yeni Ürün Fiyatı: ");
                        if (!double.TryParse(Console.ReadLine(), out double newPrice))
                        {
                            throw new FormatException("Geçersiz Yeni Ürün Fiyatı formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Yeni Ürün Stok: ");
                        if (!int.TryParse(Console.ReadLine(), out int newStock))
                        {
                            throw new FormatException("Geçersiz Yeni Ürün Stok formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Yeni Kategori ID (CategoryId): ");
                        if (!int.TryParse(Console.ReadLine(), out int newCategoryId_Product))
                        {
                            throw new FormatException("Geçersiz Yeni Kategori ID formatı. Lütfen sayısal bir değer girin.");
                        }

                        Product p2 = new()
                        {
                            Id = parsedId,
                            UId = newUId,
                            UrunAdı = newUrunAdi,
                            Description = newDescription,
                            Price = newPrice,
                            Stock = newStock,
                            CategoryId = newCategoryId_Product,
                            saveDate = DateTime.Now
                        };
                        bool p2Update = productServiceUpdate.UpdateProduct(p2);
                        if (p2Update)
                        {
                            Console.WriteLine($"Ürün başarıyla güncellendi.");
                        }
                        else
                        {
                            Console.WriteLine($"Ürün güncellenemedi. Belirtilen MongoDB ID'sine sahip ürün bulunamadı.");
                            logControl.LogError("Ürün Güncelleme - Kayıt Bulunamadı", new Exception($"Güncellenmeye çalışılan ürün bulunamadı. ID: {productIdToUpdate}"));
                        }
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Ürün Güncelleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Güncelleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "3":
                    // Ürün Silme (MongoDB)
                    Console.WriteLine("\n--- Ürün Silme ---");
                    ProductService productServiceDelete = new ProductService();
                    try
                    {
                        Console.Write("Silinecek Ürünün MongoDB ID'si (24 haneli hex string örn: 60c72834d9a3b2b8e8f8a1a2): ");
                        string productIdToDelete = Console.ReadLine()!;
                        if (!ObjectId.TryParse(productIdToDelete, out _))
                        {
                            throw new FormatException("Geçersiz MongoDB ID formatı.");
                        }
                        productServiceDelete.DeleteProductById(productIdToDelete);
                        Console.WriteLine("Ürün silme işlemi tamamlandı (başarı durumu garanti edilemez).");
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Ürün Silme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Silme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "4":
                    // Ürünleri listele 10'luk sayfalarda
                    Console.WriteLine("\n--- Ürün Listeleme ---");
                    ProductService productServiceList = new ProductService();
                    try
                    {
                        Console.Write("Sayfa Boyutu (örn: 10): ");
                        if (!int.TryParse(Console.ReadLine(), out int pageSize))
                        {
                            throw new FormatException("Geçersiz Sayfa Boyutu formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Sayfa Numarası (örn: 1): ");
                        if (!int.TryParse(Console.ReadLine(), out int pageNumber))
                        {
                            throw new FormatException("Geçersiz Sayfa Numarası formatı. Lütfen sayısal bir değer girin.");
                        }

                        ProductPage productPage = productServiceList.GetAllProductsPage(pageSize, pageNumber);
                        Console.WriteLine($"Total Count: {productPage.TotalCount}");
                        Console.WriteLine($"Total Page: {productPage.TotalPage}");
                        if (productPage.Products != null && productPage.Products.Any())
                        {
                            foreach (var product in productPage.Products)
                            {
                                Console.WriteLine($"ID: {product.Id}, User UID: {product.UId} Name: {product.UrunAdı}, Description: {product.Description}, Price: {product.Price}, Stock: {product.Stock}, CategoryId: {product.CategoryId}, SaveDate: {product.saveDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Listelenecek ürün bulunamadı.");
                        }
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Ürün Listeleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Listeleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "5":
                    // Ürünleri arama
                    Console.WriteLine("\n--- Ürün Arama ---");
                    ProductService productServiceSearch = new ProductService();
                    try
                    {
                        Console.Write("Aranacak Anahtar Kelime: ");
                        string searchKeyword = Console.ReadLine()!;
                        List<Product> searchResults = productServiceSearch.SearchProducts(searchKeyword);
                        if (searchResults.Count > 0)
                        {
                            Console.WriteLine($"Anahtar kelime '{searchKeyword}' ile eşleşen ürünler:");
                            foreach (var product in searchResults)
                            {
                                Console.WriteLine($"ID: {product.UId} Name: {product.UrunAdı} Description: {product.Description} Price: {product.Price} Stock: {product.Stock} CategoryId: {product.CategoryId} SaveDate: {product.saveDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Anahtar kelime '{searchKeyword}' ile eşleşen ürün bulunamadı.");
                            logControl.LogError("Ürün Arama - Kayıt Bulunamadı", new Exception($"'{searchKeyword}' anahtar kelimesi ile eşleşen ürün bulunamadı."));
                        }
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Arama", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "6":
                    // Ürünleri kategoriye göre filtreleme
                    Console.WriteLine("\n--- Ürün Filtreleme (Kategoriye Göre) ---");
                    ProductService productServiceFilter = new ProductService();
                    try
                    {
                        Console.Write("Filtrelenecek Kategori ID'si: ");
                        if (!int.TryParse(Console.ReadLine(), out int categoryIdToFilter))
                        {
                            throw new FormatException("Geçersiz Kategori ID'si formatı. Lütfen sayısal bir değer girin.");
                        }
                        List<Product> productsByCategory = productServiceFilter.GetProductsByCategoryId(categoryIdToFilter);
                        if (productsByCategory.Count > 0)
                        {
                            Console.WriteLine($"Kategori ID'si {categoryIdToFilter} olan ürünler:");
                            foreach (var product in productsByCategory)
                            {
                                Console.WriteLine($"ID: {product.UId} Name: {product.UrunAdı} Description: {product.Description} Price: {product.Price} Stock: {product.Stock} CategoryId: {product.CategoryId} SaveDate: {product.saveDate}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Kategori ID'si {categoryIdToFilter} olan ürün bulunamadı.");
                            logControl.LogError("Ürün Filtreleme - Kayıt Bulunamadı", new Exception($"Kategori ID'si {categoryIdToFilter} ile eşleşen ürün bulunamadı."));
                        }
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Ürün Filtreleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Ürün Filtreleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "7":
                    // Kategori Ekle
                    Console.WriteLine("\n--- Kategori Ekleme ---");
                    CategoryService categoryServiceAdd = new CategoryService();
                    try
                    {
                        Console.Write("Kategori ID (CId): ");
                        if (!int.TryParse(Console.ReadLine(), out int cId))
                        {
                            throw new FormatException("Geçersiz Kategori ID formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Kategori Adı: ");
                        string cName = Console.ReadLine()!;

                        Category c1 = new()
                        {
                            Id = ObjectId.GenerateNewId(),
                            CId = cId,
                            CName = cName
                        };
                        int c1Save = categoryServiceAdd.AddCategory(c1);
                        Console.WriteLine($"Kayıt Yapılan Sayı: {c1Save}");
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Kategori Ekleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Kategori Ekleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "8":
                    // Kategori Güncelleme (CId'ye göre)
                    Console.WriteLine("\n--- Kategori Güncelleme (CId'ye Göre) ---");
                    CategoryService categoryServiceUpdate = new CategoryService();
                    try
                    {
                        Console.Write("Güncellenecek Kategori ID (CId): ");
                        if (!int.TryParse(Console.ReadLine(), out int cIdToUpdate))
                        {
                            throw new FormatException("Geçersiz Kategori ID formatı. Lütfen sayısal bir değer girin.");
                        }
                        Console.Write("Yeni Kategori Adı: ");
                        string newCName = Console.ReadLine()!;

                        bool c2Update = categoryServiceUpdate.UpdateCategory(cIdToUpdate, newCName);
                        if (c2Update)
                        {
                            Console.WriteLine($"Kategori başarıyla güncellendi.");
                        }
                        else
                        {
                            Console.WriteLine($"Kategori güncellenemedi. Belirtilen Kategori ID'sine sahip kategori bulunamadı.");
                            logControl.LogError("Kategori Güncelleme - Kayıt Bulunamadı", new Exception($"Güncellenmeye çalışılan kategori bulunamadı. ID: {cIdToUpdate}"));
                        }
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Kategori Güncelleme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Kategori Güncelleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;
                    
                case "9":
                    // Kategori Silme (MongoDB)
                    Console.WriteLine("\n--- Kategori Silme ---");
                    var categoryServiceDelete = new CategoryService();
                    try
                    {
                        Console.Write("Silinecek Kategori MongoDB ID'si (24 haneli hex string örn: 60c72834d9a3b2b8e8f8a1a2): ");
                        string categoryIdToDelete = Console.ReadLine()!;
                        if (!ObjectId.TryParse(categoryIdToDelete, out _))
                        {
                            throw new FormatException("Geçersiz MongoDB ID formatı.");
                        }

                        categoryServiceDelete.DeleteCategory(categoryIdToDelete);
                        Console.WriteLine("Kategori silme işlemi tamamlandı (başarı durumu garanti edilemez).");
                    }
                    catch (FormatException ex)
                    {
                        logControl.LogError("Kategori Silme - Veri Giriş Hatası", ex);
                        Console.WriteLine($"Hata: {ex.Message} Hata log dosyasına kaydedildi.");
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Kategori Silme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "10":
                    // Kategori Listeleme
                    Console.WriteLine("\n--- Kategori Listeleme ---");
                    var categoryList = new CategoryService();
                    try
                    {
                        List<Category> categories = categoryList.GetAllCategories();
                        if (categories.Any())
                        {
                            foreach (var category in categories)
                            {
                                Console.WriteLine($"MongoDB ID: {category.Id}, Sizin ID: {category.CId} Name: {category.CName}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Listelenecek kategori bulunamadı.");
                        }
                    }
                    catch (Exception ex)
                    {
                        logControl.LogError("Kategori Listeleme", ex);
                        Console.WriteLine($"Beklenmeyen bir hata oluştu: {ex.Message}. Hata log dosyasına kaydedildi.");
                    }
                    break;

                case "11":
                // Hata Loglarını Görüntüleme
                    Console.WriteLine("\n--- Hata Loglarını Görüntüleme ---");
                    List<string> allLogs = logControl.GetAllLogs();

                    if (allLogs.Count > 0)
                    {
                        Console.WriteLine("Tüm Log Kayıtları:");
                        foreach (var log in allLogs)
                        {
                            Console.WriteLine(log);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Görüntülenecek log kaydı bulunamadı.");
                    }
                    break;

                case "12":
                // Hata Loglarında Arama Yapma
                    Console.WriteLine("\n--- Hata Loglarında Arama Yapma ---");
                    Console.Write("Aranacak metni girin: ");
                    string searchText = Console.ReadLine()!;
                    List<string> searchResultsInLogs = logControl.SearchInLogs(searchText);

                    if (searchResultsInLogs.Count > 0)
                    {
                        Console.WriteLine($"'{searchText}' ile eşleşen log kayıtları:");
                        foreach (var log in searchResultsInLogs)
                        {
                            Console.WriteLine(log);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"'{searchText}' ifadesi ile eşleşen log bulunamadı.");
                    }
                    break;
                default:
                // Geçersiz seçim
                    Console.WriteLine("Geçersiz seçim. Lütfen 1 ile 12 arasında bir sayı girin.");
                    logControl.LogError("Main Menü Seçimi", new ArgumentOutOfRangeException("choice", $"Geçersiz menü seçimi yapıldı: {choice}"));
                    break;
            }
        }
    }
}