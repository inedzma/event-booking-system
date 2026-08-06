using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Repositories
{
	public class BookingRepository : IBookingRepository
	{
		private readonly EventBookingDbContext _context;

		public BookingRepository(EventBookingDbContext context)
		{
			_context = context;
		}

		public void Add(Booking entity)
		{
			_context.Bookings.Add(entity);
			_context.SaveChanges();
		}

		public void Update(Booking entity)
		{
			_context.Bookings.Update(entity);
			_context.SaveChanges();
		}

		public List<Booking> GetAll()
		{
			return _context.Bookings.ToList();
		}

		public Booking? GetById(int id)
		{
			return _context.Bookings.Find(id);
		}
	}
}