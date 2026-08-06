using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Repositories
{
	public interface IBookingRepository
	{
		void Add(Booking entity);
		void Update(Booking entity);
		List<Booking> GetAll();
		Booking? GetById(int id);
	}
}