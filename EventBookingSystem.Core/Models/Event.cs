using System.Net.Sockets;

namespace EventBookingSystem.Core.Models
{
	public abstract class Event
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime Date { get; set; }
		public string Location { get; set; } = string.Empty;
		public int Capacity { get; set; }

		// Organizator eventa - postavlja se kad Booking bude odobren
		public int OrganizerId { get; set; }
		public User Organizer { get; set; } = null!;

		public List<Ticket> Tickets { get; set; } = new();

		public abstract string GetEventDetails();
	}


}