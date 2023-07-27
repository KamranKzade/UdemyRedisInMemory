﻿using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repository;
using RedisExampleApp.Cache;
using StackExchange.Redis;

namespace RedisExampleApp.API.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly IDatabase _database;
	private readonly IProductRepository _productRepository;


	public ProductsController(IProductRepository productRepository, IDatabase database)
	{
		_productRepository = productRepository;
		_database = database;
		_database.StringSet("soyad", "karimzada");
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
