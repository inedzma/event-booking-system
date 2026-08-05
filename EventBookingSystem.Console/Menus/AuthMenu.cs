using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class AuthMenu
	{
		private readonly IUserService _userService;

		public AuthMenu(IUserService userService)
		{
			_userService = userService;
		}

		public User? Run()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== EVENT BOOKING SYSTEM ===");
			System.Console.WriteLine("1. Login");
			System.Console.WriteLine("2. Register");
			System.Console.Write("Choose an option: ");
			string? choice = System.Console.ReadLine();

			return choice switch
			{
				"1" => Login(),
				"2" => Register(),
				_ => Invalid()
			};
		}

		private User? Login()
		{
			System.Console.Write("Email: ");
			string email = System.Console.ReadLine() ?? "";
			System.Console.Write("Password: ");
			string password = System.Console.ReadLine() ?? "";

			var user = _userService.Login(email, password);

			if (user == null)
			{
				System.Console.WriteLine("Invalid email or password.");
				return null;
			}

			return user;
		}

		private User? Register()
		{
			System.Console.Write("Name: ");
			string name = System.Console.ReadLine() ?? "";
			System.Console.Write("Email: ");
			string email = System.Console.ReadLine() ?? "";
			System.Console.Write("Password: ");
			string password = System.Console.ReadLine() ?? "";

			try
			{
				var user = _userService.Register(name, email, password);
				System.Console.WriteLine("Registration successful!");
				return user;
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error: {ex.Message}");
				return null;
			}
		}

		private User? Invalid()
		{
			System.Console.WriteLine("Invalid choice.");
			return null;
		}
	}
}