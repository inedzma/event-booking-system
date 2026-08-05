using EventBookingSystem.Core.Data;
using EventBookingSystem.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace EventBookingSystem.Core.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly EventBookingDbContext _context;

		public UserRepository(EventBookingDbContext context)
		{
			_context = context;
		}

		public void Add(User entity)
		{
			_context.Users.Add(entity);
			_context.SaveChanges();
		}

		public void Update(User entity)
		{
			_context.Users.Update(entity);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var user = _context.Users.Find(id)
				?? throw new KeyNotFoundException($"Korisnik sa Id {id} ne postoji.");
			_context.Users.Remove(user);
			_context.SaveChanges();
		}

		public List<User> GetAll()
		{
			return _context.Users.ToList();
		}

		public User? GetById(int id)
		{
			return _context.Users.Find(id);
		}

		public User? GetByEmail(string email)
		{
			return _context.Users
				.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
		}
	}
}