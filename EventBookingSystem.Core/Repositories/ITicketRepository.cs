using EventBookingSystem.Core.Models;
using System.Collections.Generic;

namespace EventBookingSystem.Core.Repositories
{
	public interface ITicketRepository
	{
		void Add(Ticket entity);
		void Update(Ticket entity);
		void Delete(int id);
		List<Ticket> GetAll();
		Ticket? GetById(int id);
		List<Ticket> GetByUserId(int userId);
		List<Ticket> GetByEventId(int eventId);
	}
}