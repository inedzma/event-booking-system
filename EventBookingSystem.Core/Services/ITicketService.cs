using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public interface ITicketService
	{
		Ticket BuyTicket(int eventId, int userId, string categoryName);
		void CancelTicket(int ticketId, int userId);
		List<Ticket> GetUserTickets(int userId);

		List<Ticket> GetAllValidTickets();
	}
}