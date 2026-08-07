using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public class ReportService : IReportService
	{
		private readonly IEventService _eventService;
		private readonly ITicketService _ticketService;

		public ReportService(IEventService eventService, ITicketService ticketService)
		{
			_eventService = eventService;
			_ticketService = ticketService;
		}

		// Pretraga po nazivu, case-insensitive
		public List<Event> SearchByTitle(string keyword)
		{
			if (string.IsNullOrWhiteSpace(keyword))
				throw new ArgumentException("Pojam za pretragu ne smije biti prazan.");

			return _eventService.GetAll()
				.Where(e => e.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase))
				.ToList();
		}

		// Filtriranje eventa koji se dešavaju u narednih X dana, X dolazi kao string
		public List<Event> FilterByDaysFromNow(string daysInput)
		{
			if (!int.TryParse(daysInput, out int days) || days < 0)
				throw new ArgumentException($"'{daysInput}' nije validan broj dana.");

			var limitDate = DateTime.Now.AddDays(days);

			return _eventService.GetAll()
				.Where(e => e.Date >= DateTime.Now && e.Date <= limitDate)
				.OrderBy(e => e.Date)
				.ToList();
		}

		// Grupisanje eventa po tipu (Concert/Conference/Workshop)
		public Dictionary<string, int> GroupEventsByCategory()
		{
			return _eventService.GetAll()
				.GroupBy(e => e.GetType().Name)
				.ToDictionary(g => g.Key, g => g.Count());
		}

		// Grupisanje prodatih karata po eventu (projekcija naziva eventa)
		public Dictionary<string, int> GroupTicketsByEvent()
		{
			var allEvents = _eventService.GetAll();
			var allTickets = _ticketService.GetAllValidTickets();

			return allTickets
				.GroupBy(t => t.EventId)
				.Select(g => new
				{
					Title = allEvents.FirstOrDefault(e => e.Id == g.Key)?.Title ?? "Unknown",
					Count = g.Count()
				})
				.ToDictionary(x => x.Title, x => x.Count);
		}

		// Izvještaj o zaradi po eventu
		public List<(Event Event, decimal TotalRevenue)> RevenueReport()
		{
			var allEvents = _eventService.GetAll();
			var allTickets = _ticketService.GetAllValidTickets();

			return allEvents
				.Select(e => (
					Event: e,
					TotalRevenue: allTickets.Where(t => t.EventId == e.Id).Sum(t => t.Price)
				))
				.OrderByDescending(x => x.TotalRevenue)
				.ToList();
		}

		// Najpopularniji event po broju prodatih karata
		public Event? MostPopularEvent()
		{
			var allEvents = _eventService.GetAll();
			var allTickets = _ticketService.GetAllValidTickets();

			var grouped = allTickets
				.GroupBy(t => t.EventId)
				.OrderByDescending(g => g.Count())
				.FirstOrDefault();

			if (grouped == null) return null;

			return allEvents.FirstOrDefault(e => e.Id == grouped.Key);
		}

		public List<(Event Event, int SoldTickets, int RemainingSeats)> AvailabilityReport()
		{
			var allEvents = _eventService.GetAll();
			var allTickets = _ticketService.GetAllValidTickets();

			return allEvents
				.Select(e =>
				{
					int sold = allTickets.Count(t => t.EventId == e.Id);
					return (Event: e, SoldTickets: sold, RemainingSeats: e.Capacity - sold);
				})
				.OrderBy(x => x.RemainingSeats)
				.ToList();
		}
	}
}