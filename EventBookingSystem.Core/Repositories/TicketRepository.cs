using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Models;
using System.Collections.Generic;
using System.Linq;

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

		public void Delete(int id)
		{
			var ticket = _context.Tickets.Find(id)
				?? throw new KeyNotFoundException($"Ticket sa Id {id} ne postoji.");
			_context.Tickets.Remove(ticket);
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

		public List<Ticket> GetByUserId(int userId)
		{
			return _context.Tickets.Where(t => t.UserId == userId).ToList();
		}

		public List<Ticket> GetByEventId(int eventId)
		{
			return _context.Tickets.Where(t => t.EventId == eventId).ToList();
		}
	}
}