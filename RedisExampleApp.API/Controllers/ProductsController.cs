using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;


namespace RedisExampleApp.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	// Productlardan istifade etmek ucun
	private readonly IProductRepository _productRepository;

	public ProductsController(IProductRepository productRepository)
	{
		_productRepository = productRepository;
	}


	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await _productRepository.GetAsync());
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		return Ok(await _productRepository.GetByIdAsync(id));
	}

	[HttpPost]
	public async Task<IActionResult> Create(Product product)
	{
		return Created(string.Empty, await _productRepository.CreateAsync(product));
	}
}
