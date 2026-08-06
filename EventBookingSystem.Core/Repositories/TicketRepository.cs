using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Repositories
{
	public class TicketRepository : ITicketRepository
	{
		private readonly EventBookingDbContext _context;

		public TicketRepository(EventBookingDbContext context)
		{
			_context = context;
		}

		public void Add(Ticket entity)
		{
			_context.Tickets.Add(entity);
			_context.SaveChanges();
		}

		public void Update(Ticket entity)
		{
			_context.Tickets.Update(entity);
			_context.SaveChanges();
		}

		public List<Ticket> GetAll()
		{
			return _context.Tickets.ToList();
		}

		public Ticket? GetById(int id)
		{
			return _context.Tickets.Find(id);
		}
	}
}