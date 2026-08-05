using EventBookingSystem.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
	.AddDbContext<EventBookingDbContext>()
	.BuildServiceProvider();

using (var scope = services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<EventBookingDbContext>();
	dbContext.Database.EnsureCreated();
}

Console.WriteLine("Baza je uspješno kreirana!");