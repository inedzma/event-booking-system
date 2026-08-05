using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventBookingSystem.Core.Repositories
{
	public class EventRepository : IEventRepository
	{
		private readonly EventBookingDbContext _context;

		public EventRepository(EventBookingDbContext context)
		{
			_context = context;
		}

		public void Add(Event entity)
		{
			_context.Events.Add(entity);
			_context.SaveChanges();
		}

		public void Update(Event entity)
		{
			_context.Events.Update(entity);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var ev = _context.Events.Find(id)
				?? throw new KeyNotFoundException($"Event sa Id {id} ne postoji.");
			_context.Events.Remove(ev);
			_context.SaveChanges();
		}

		public List<Event> GetAll()
		{
			return _context.Events.ToList();
		}

		public Event? GetById(int id)
		{
			return _context.Events.Find(id);
		}
	}
}