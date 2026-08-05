using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Console.Menus
{
	public class MainMenu
	{
		private readonly User _currentUser;

		public MainMenu(User currentUser)
		{
			_currentUser = currentUser;
		}

		public void Run()
		{
			bool exit = false;

			while (!exit)
			{
				System.Console.Clear();

				if (_currentUser.Role == UserRole.Admin)
				{
					System.Console.WriteLine($"Welcome Admin {_currentUser.Name}!\n");
					ShowAdminOptions();
				}
				else
				{
					System.Console.WriteLine($"Welcome {_currentUser.Name}!\n");
					ShowUserOptions();
				}

				System.Console.Write("\nChoose an option: ");
				string? choice = System.Console.ReadLine();

				if (choice == "0")
				{
					exit = true;
					continue;
				}

				// Ovdje ćemo kasnije dodati switch za pozivanje pravih akcija
				System.Console.WriteLine("\nFeature not implemented yet.");
				Pause();
			}
		}

		private void ShowAdminOptions()
		{
			System.Console.WriteLine("1. Manage Events");
			System.Console.WriteLine("2. View All Bookings");
			System.Console.WriteLine("3. View Reports");
			System.Console.WriteLine("0. Logout");
		}

		private void ShowUserOptions()
		{
			System.Console.WriteLine("1. Browse Events");
			System.Console.WriteLine("2. Buy Ticket");
			System.Console.WriteLine("3. My Tickets");
			System.Console.WriteLine("0. Logout");
		}

		private void Pause()
		{
			System.Console.WriteLine("Press any key to continue...");
			System.Console.ReadKey();
		}
	}
}