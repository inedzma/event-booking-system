using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;

namespace EventBookingSystem.Core.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public User Register(string name, string email, string password)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentException("Ime ne smije biti prazno.");

			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email ne smije biti prazan.");

			if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
				throw new ArgumentException("Lozinka mora imati barem 4 karaktera.");

			if (_userRepository.GetByEmail(email) != null)
				throw new InvalidOperationException("Korisnik sa ovim emailom već postoji.");

			var newUser = new User
			{
				Name = name,
				Email = email,
				Password = password,
				Role = UserRole.User
			};

			_userRepository.Add(newUser);
			return newUser;
		}

		public User? Login(string email, string password)
		{
			var user = _userRepository.GetByEmail(email);

			if (user == null || user.Password != password)
				return null;

			return user;
		}
	}
}