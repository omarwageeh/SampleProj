using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;

namespace SampleApi.Tests
{
	public class CustomWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Remove all DbContextOptions registrations
				var dbContextOptions = services.Where(
					d => d.ServiceType.Name.Contains("DbContextOptions")).ToList();
				foreach (var d in dbContextOptions)
					services.Remove(d);

				// Add in-memory database
				services.AddDbContext<AppDbContext>(options =>
				{
					options.UseInMemoryDatabase("TestDb");
				});
			});
		}
	}
}