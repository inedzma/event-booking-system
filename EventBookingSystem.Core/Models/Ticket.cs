using EventBookingSystem.Core.Enums;

namespace EventBookingSystem.Core.Models
{
	public class Ticket
	{
		public int Id { get; set; }

		public int EventId { get; set; }
		public Event Event { get; set; } = null!;

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public DateTime PurchaseDate { get; set; }
		public decimal Price { get; set; }
		public TicketStatus Status { get; set; }
	}
}