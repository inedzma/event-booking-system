using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Repositories
{
	public interface ITicketRepository
	{
		void Add(Ticket entity);
		void Update(Ticket entity);
		List<Ticket> GetAll();
		Ticket? GetById(int id);
	}
}