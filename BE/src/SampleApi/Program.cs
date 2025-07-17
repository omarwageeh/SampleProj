using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces;
using Infrastructure.Repositories;
using Application.Common;
using Application.Products.Commands;
using Application.Products.Dtos;
using Application.Products.Handlers;
using Application.Products.Queries;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseMySql(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
	)
);

// Add CORS policy
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins("http://localhost:3000")
			  .AllowAnyHeader()
			  .AllowAnyMethod();
	});
});

// Register repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register CQRS handlers
builder.Services.AddScoped<ICommandHandler<CreateProductCommand, ProductDto>, CreateProductHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateProductCommand, bool>, UpdateProductHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteProductCommand, bool>, DeleteProductHandler>();
builder.Services.AddScoped<IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>, GetProductsHandler>();
builder.Services.AddScoped<IQueryHandler<GetProductByIdQuery, ProductDto?>, GetProductByIdHandler>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Use CORS
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Serve static files from wwwroot (for images)
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

public partial class Program;