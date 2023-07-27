using System.Text.Json;
using StackExchange.Redis;
using RedisExampleApp.Cache;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository.Abstract;

namespace RedisExampleApp.API.Repository.Concrete;


// Cache ile islemek ucun lazim olan classdir, esas baxilan yer Controller ozunde neyi property kimi
// saxlayirsa onun aid oldugu yere gedib, saxladigi propertynin istifade etdiyi interface-den 
// yeni 1 class yaratmaqdir.Burda esas diqqet olunasi yer, yaratdigimiz classinda hemin interface-den
// yaranmagidir.
public class ProductRepositoryWithCacheDecorator : IProductRepository
{
    // Redis olacaq Cache ismim
    private const string productKey = "productCaches";
    // Redis-i istifade ede bilek deye
    private readonly RedisService _service;
    // Repolari Redis ile elaqesini yarada bilek deye
    private readonly IProductRepository _repository;

    // Redis-de hansi db-de tutacagimizi gostermek ucun
    private readonly IDatabase _cacheRepository;

    public ProductRepositoryWithCacheDecorator(RedisService service, IProductRepository repository)
    {
        _service = service;
        _repository = repository;
        _cacheRepository = _service.GetDb(2);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var newproduct = await _repository.CreateAsync(product);

        // Sualli olan hisse
        if (await _cacheRepository.KeyExistsAsync(productKey))
        {
            await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newproduct));
        }

        return newproduct;
    }

    public async Task<List<Product>> GetAsync()
    {
        // Eger data Cache-de yoxdursa, onu cache-leyib geri qaytarir
        if (!await _cacheRepository.KeyExistsAsync(productKey))
            return await LoadToCacheFromDbAsync();

        // Eger data Cache-de varsa
        var products = new List<Product>();
        // Cache-den oxuyub geri qaytarir datalari
        var cacheProduct = await _cacheRepository.HashGetAllAsync(productKey);
        foreach (var item in cacheProduct.ToList())
        {
            var product = JsonSerializer.Deserialize<Product>(item.Value);
            products.Add(product);
        }

        return products;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        // Eger data Cache-de varsa, onu cache-den geri qaytarir
        if (await _cacheRepository.KeyExistsAsync(productKey))
        {
            var product = await _cacheRepository.HashGetAsync(productKey, id);
            return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
        }

        // Eger data Cache-de yoxdursa, cache-leyirik ve uygun datani geri qaytaririq
        var products = await LoadToCacheFromDbAsync();
        return products.FirstOrDefault(x => x.Id == id);
    }

    private async Task<List<Product>> LoadToCacheFromDbAsync()
    {
        var products = await _repository.GetAsync();

        // Gelen Productlari Cache-leyeceyik
        products.ForEach(p =>
        {
            _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
        });

        return products;
    }

}