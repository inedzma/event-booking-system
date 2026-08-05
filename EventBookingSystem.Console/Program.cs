using EventBookingSystem.Console.Menus;
using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;
using EventBookingSystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
	.AddDbContext<EventBookingDbContext>()
	.AddScoped<IUserRepository, UserRepository>()
	.AddScoped<IUserService, UserService>()
	.AddScoped<IEventRepository, EventRepository>()
	.AddScoped<IEventService, EventService>()
	.AddScoped<ITicketRepository, TicketRepository>()
	.AddScoped<ITicketService, TicketService>()
	.BuildServiceProvider();

using (var scope = services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<EventBookingDbContext>();
	dbContext.Database.EnsureCreated();

	if (!dbContext.Users.Any(u => u.Role == UserRole.Admin))
	{
		dbContext.Users.Add(new User
		{
			Name = "Admin",
			Email = "admin@eventbooking.com",
			Password = "admin123",
			Role = UserRole.Admin
		});
		dbContext.SaveChanges();
	}
}

var userService = services.GetRequiredService<IUserService>();

var authMenu = new AuthMenu(userService);
var currentUser = authMenu.Run();

if (currentUser == null)
{
	return;
}

var eventService = services.GetRequiredService<IEventService>();

var ticketService = services.GetRequiredService<ITicketService>();

var mainMenu = new MainMenu(currentUser, eventService, ticketService);
mainMenu.Run();