using RedisExampleApp.API.Models;
using RedisExampleApp.Cache;

namespace RedisExampleApp.API.Repository;

// Cache ile islemek ucun lazim olan classdir, esas baxilan yer Controller ozunde neyi property kimi
// saxlayirsa onun aid oldugu yere gedib, saxladigi propertynin istifade etdiyi interface-den 
// yeni 1 class yaratmaqdir.Burda esas diqqet olunasi yer, yaratdigimiz classinda hemin interface-den
// yaranmagidir.
public class ProductRepositoryWithCache : IProductRepository
{
	// Redis-i istifade ede bilek deye
	private readonly RedisService _service;
	// Repolari Redis ile elaqesini yarada bilek deye
	private readonly IProductRepository _repository;

	public ProductRepositoryWithCache(RedisService service, IProductRepository repository)
	{
		_service = service;
		_repository = repository;
	}

	public Task<Product> CreateAsync(Product product)
	{
		throw new NotImplementedException();
	}

	public Task<List<Product>> GetAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Product> GetByIdAsync(int id)
	{
		throw new NotImplementedException();
	}
}
