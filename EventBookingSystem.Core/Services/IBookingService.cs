using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public interface IBookingService
	{
		Booking RequestBooking(int userId, string title, EventCategory category, DateTime date, string location, int capacity);
		List<Booking> GetPending();
		List<Booking> GetAll();
		void Reject(int bookingId);
		Event Approve(int bookingId, string? performer, string? topic, int? numberOfSpeakers, int? maxParticipantsPerGroup);
	}
}