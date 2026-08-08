using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;
using EventBookingSystem.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class App : Application
	{
		public static IServiceProvider Services { get; private set; } = null!;

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var services = new ServiceCollection()
				.AddDbContext<EventBookingDbContext>()
				.AddScoped<IUserRepository, UserRepository>()
				.AddScoped<IUserService, UserService>()
				.AddScoped<IEventRepository, EventRepository>()
				.AddScoped<IEventService, EventService>()
				.AddScoped<ITicketRepository, TicketRepository>()
				.AddScoped<ITicketService, TicketService>()
				.AddScoped<IBookingRepository, BookingRepository>()
				.AddScoped<IBookingService, BookingService>()
				.AddScoped<IReportService, ReportService>()
				.BuildServiceProvider();

			Services = services;

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

			var loginWindow = new LoginWindow();
			loginWindow.Show();
		}
	}
}