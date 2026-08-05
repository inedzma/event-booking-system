using EventBookingSystem.Core.Enums;

namespace EventBookingSystem.Core.Models
{
	public class Booking
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public int EventId { get; set; }
		public Event Event { get; set; } = null!;

		public DateTime BookingDate { get; set; }
		public BookingStatus Status { get; set; }
	}
}