using System.Net.Http.Json;

namespace SampleApi.Tests
{
	public class ProductApiTests : IClassFixture<CustomWebApplicationFactory>
	{
		private readonly CustomWebApplicationFactory _factory;
		public ProductApiTests(CustomWebApplicationFactory factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task Can_Create_And_Get_Product()
		{
			var client = _factory.CreateClient();
			var createDto = new { name = "Test Product", description = "A test product", price = 99.99M };
			// Create
			var postResponse = await client.PostAsJsonAsync("/api/products", createDto);
			postResponse.EnsureSuccessStatusCode();
			var created = await postResponse.Content.ReadFromJsonAsync<ProductDto>();
			Assert.NotNull(created);
			Assert.Equal("Test Product", created!.Name);
			// Get All
			var getAllResponse = await client.GetAsync("/api/products");
			getAllResponse.EnsureSuccessStatusCode();
			var products = await getAllResponse.Content.ReadFromJsonAsync<ProductDto[]>();
			Assert.Contains(products!, p => p.Id == created.Id);
			// Get By Id
			var getByIdResponse = await client.GetAsync($"/api/products/{created.Id}");
			getByIdResponse.EnsureSuccessStatusCode();
			var byId = await getByIdResponse.Content.ReadFromJsonAsync<ProductDto>();
			Assert.Equal(created.Id, byId!.Id);
		}

		[Fact]
		public async Task Can_Update_Product()
		{
			var client = _factory.CreateClient();
			// Create
			var createDto = new { name = "ToUpdate", description = "Before update", price = 10M };
			var postResponse = await client.PostAsJsonAsync("/api/products", createDto);
			postResponse.EnsureSuccessStatusCode();
			var created = await postResponse.Content.ReadFromJsonAsync<ProductDto>();
			// Update
			var updateDto = new { id = created!.Id, name = "Updated Name", description = "Updated desc", price = 20M };
			var putResponse = await client.PutAsJsonAsync($"/api/products/{created.Id}", updateDto);
			putResponse.EnsureSuccessStatusCode();
			// Get By Id
			var getByIdResponse = await client.GetAsync($"/api/products/{created.Id}");
			getByIdResponse.EnsureSuccessStatusCode();
			var updated = await getByIdResponse.Content.ReadFromJsonAsync<ProductDto>();
			Assert.Equal("Updated Name", updated!.Name);
			Assert.Equal("Updated desc", updated.Description);
			Assert.Equal(20M, updated.Price);
		}

		[Fact]
		public async Task Can_Delete_Product()
		{
			var client = _factory.CreateClient();
			// Create
			var createDto = new { name = "ToDelete", description = "To be deleted", price = 5M };
			var postResponse = await client.PostAsJsonAsync("/api/products", createDto);
			postResponse.EnsureSuccessStatusCode();
			var created = await postResponse.Content.ReadFromJsonAsync<ProductDto>();
			// Delete
			var deleteResponse = await client.DeleteAsync($"/api/products/{created!.Id}");
			deleteResponse.EnsureSuccessStatusCode();
			// Get By Id (should be 404)
			var getByIdResponse = await client.GetAsync($"/api/products/{created.Id}");
			Assert.Equal(System.Net.HttpStatusCode.NotFound, getByIdResponse.StatusCode);
		}

		public class ProductDto
		{
			public int Id { get; set; }
			public string Name { get; set; } = string.Empty;
			public string Description { get; set; } = string.Empty;
			public decimal Price { get; set; }
		}
	}
}