using EventBookingSystem.Core.Models;
using System.Collections.Generic;

namespace EventBookingSystem.Core.Services
{
	public interface ITicketService
	{
		Ticket BuyTicket(int userId, int eventId, decimal price);
		void CancelTicket(int ticketId);
		List<Ticket> GetTicketsByUser(int userId);
	}
}