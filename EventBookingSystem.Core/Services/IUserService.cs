using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public interface IUserService
	{
		User Register(string name, string email, string password);
		User? Login(string email, string password);
	}
}