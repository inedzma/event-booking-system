using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;

namespace EventBookingSystem.Core.Services
{
	public class TicketService : ITicketService
	{
		private readonly ITicketRepository _ticketRepository;
		private readonly IEventService _eventService;

		public TicketService(ITicketRepository ticketRepository, IEventService eventService)
		{
			_ticketRepository = ticketRepository;
			_eventService = eventService;
		}

		public Ticket BuyTicket(int eventId, int userId, string categoryName)
		{
			var ev = _eventService.GetById(eventId);

			var option = ev.GetTicketOptions()
				.FirstOrDefault(o => o.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase))
				?? throw new ArgumentException("Nepostojeća kategorija karte za ovaj event.");

			int soldTickets = _ticketRepository.GetAll()
				.Count(t => t.EventId == eventId && t.Status == TicketStatus.Valid);

			if (soldTickets >= ev.Capacity)
				throw new InvalidOperationException("Nema više slobodnih mjesta za ovaj event.");

			var ticket = new Ticket
			{
				EventId = eventId,
				UserId = userId,
				Category = option.Name,
				Price = option.Price,
				PurchaseDate = DateTime.Now,
				Status = TicketStatus.Valid
			};

			_ticketRepository.Add(ticket);
			return ticket;
		}

		public void CancelTicket(int ticketId, int userId)
		{
			var ticket = _ticketRepository.GetById(ticketId)
				?? throw new KeyNotFoundException("Karta ne postoji.");

			if (ticket.UserId != userId)
				throw new UnauthorizedAccessException("Ne možete otkazati tuđu kartu.");

			ticket.Status = TicketStatus.Cancelled;
			_ticketRepository.Update(ticket);
		}

		public List<Ticket> GetUserTickets(int userId)
		{
			return _ticketRepository.GetAll()
				.Where(t => t.UserId == userId)
				.ToList();
		}
	}
}