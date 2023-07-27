using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;

namespace RedisExampleApp.API.Services;

public class ProductService : IProductService
{
	private readonly IProductRepository _repo;

	public ProductService(IProductRepository repo)
	{
		_repo = repo;
	}

	public async Task<Product> CreateAsync(Product product)
	{
		return await _repo.CreateAsync(product);
	}

	public async Task<List<Product>> GetAsync()
	{
		return await _repo.GetAsync();
	}

	public async Task<Product> GetByIdAsync(int id)
	{
		var product = await _repo.GetByIdAsync(id);

		// mapper Dto
		return product;
	}
}
