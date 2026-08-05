using System.Net.Sockets;

namespace EventBookingSystem.Core.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public UserRole Role { get; set; }

		public List<Booking> Bookings { get; set; } = new();
		public List<Ticket> Tickets { get; set; } = new();
	}
}