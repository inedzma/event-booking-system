using EventBookingSystem.Core.Enums;

namespace EventBookingSystem.Core.Models
{
	public class Booking
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public User User { get; set; } = null!;

		public string ProposedTitle { get; set; } = string.Empty;
		public EventCategory Category { get; set; }
		public DateTime ProposedDate { get; set; }
		public string ProposedLocation { get; set; } = string.Empty;
		public int ProposedCapacity { get; set; }

		public BookingStatus Status { get; set; }
		public DateTime RequestDate { get; set; }
	}
}