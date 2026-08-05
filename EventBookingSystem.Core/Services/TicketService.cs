using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;

namespace EventBookingSystem.Core.Services
{
	public class TicketService : ITicketService
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly IEventRepository _eventRepository;

		public TicketService(ITicketRepository ticketRepository, IEventRepository eventRepository)
		{
			_ticketRepository = ticketRepository;
			_eventRepository = eventRepository;
		}

		public Ticket BuyTicket(int userId, int eventId, decimal price)
		{
			var ev = _eventRepository.GetById(eventId)
				?? throw new KeyNotFoundException("Event ne postoji.");

			var soldTickets = _ticketRepository.GetByEventId(eventId)
				.Count(t => t.Status != TicketStatus.Cancelled);

			if (soldTickets >= ev.Capacity)
				throw new InvalidOperationException("Nema slobodnih mjesta za ovaj event.");

			if (price < 0)
				throw new ArgumentException("Cijena ne može biti negativna.");

			var ticket = new Ticket
			{
				UserId = userId,
				EventId = eventId,
				Price = price,
				PurchaseDate = DateTime.Now,
				Status = TicketStatus.Valid
			};

			_ticketRepository.Add(ticket);
			return ticket;
		}

		public void CancelTicket(int ticketId)
		{
			var ticket = _ticketRepository.GetById(ticketId)
				?? throw new KeyNotFoundException("Karta ne postoji.");

			if (ticket.Status == TicketStatus.Cancelled)
				throw new InvalidOperationException("Karta je već otkazana.");

			ticket.Status = TicketStatus.Cancelled;
			_ticketRepository.Update(ticket);
		}

		public List<Ticket> GetTicketsByUser(int userId)
		{
			return _ticketRepository.GetByUserId(userId);
		}
	}
}