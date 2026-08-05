using EventBookingSystem.Core.Models;
using System.Collections.Generic;

namespace EventBookingSystem.Core.Repositories
{
	public interface IEventRepository
	{
		void Add(Event entity);
		void Update(Event entity);
		void Delete(int id);
		List<Event> GetAll();
		Event? GetById(int id);
	}
}