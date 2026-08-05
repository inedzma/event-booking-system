using EventBookingSystem.Core.Models;
using System.Collections.Generic;

namespace EventBookingSystem.Core.Repositories
{
	public interface IUserRepository
	{
		void Add(User entity);
		void Update(User entity);
		void Delete(int id);
		List<User> GetAll();
		User? GetById(int id);
		User? GetByEmail(string email);
	}
}