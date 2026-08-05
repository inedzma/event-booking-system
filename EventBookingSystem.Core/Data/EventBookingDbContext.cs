using EventBookingSystem.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventBookingSystem.Core.Data
{
	public class EventBookingDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Event> Events { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<Ticket> Tickets { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var putanja = Path.Combine(AppContext.BaseDirectory, "eventbooking.db");
			optionsBuilder.UseSqlite($"Data Source={putanja}");
		}
	}
}