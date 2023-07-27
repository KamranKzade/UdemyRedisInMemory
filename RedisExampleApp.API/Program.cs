using RedisExampleApp.Cache;
using RedisExampleApp.API.Models;
using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Repository;
using RedisExampleApp.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IDatabase e catmagin diger yolu
// builder.Services.AddSingleton<IDatabase>(sp =>
// {
// 	var redisService = sp.GetRequiredService<RedisService>();
// 	return redisService.GetDb(0);
// });

builder.Services.AddScoped<IProductRepository>(sp =>
{
	var appDbContext = sp.GetRequiredService<AppDbContext>();

	var productRepo = new ProductRepository(appDbContext);
	var redisService = sp.GetRequiredService<RedisService>();

	return new ProductRepositoryWithCacheDecorator(redisService, productRepo);
});

builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseInMemoryDatabase("myDatabase");
});

// Appsettings de saxladigimiz port ve hostu aliriq ve Service-lere elave edirik
builder.Services.AddSingleton<RedisService>(sp =>
{
	return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();