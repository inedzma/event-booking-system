using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class UserMenu
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;
		private readonly ITicketService _ticketService;

		public UserMenu(User currentUser, IEventService eventService, ITicketService ticketService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
			_ticketService = ticketService;
		}

		public void Run()
		{
			bool exit = false;

			while (!exit)
			{
				System.Console.Clear();
				System.Console.WriteLine($"Welcome {_currentUser.Name}!\n");
				System.Console.WriteLine("1. Browse Events");
				System.Console.WriteLine("2. Buy Ticket");
				System.Console.WriteLine("3. My Tickets");
				System.Console.WriteLine("4. Cancel Ticket");
				System.Console.WriteLine("0. Logout");
				System.Console.Write("\nChoose an option: ");

				string? choice = System.Console.ReadLine();

				switch (choice)
				{
					case "1":
						BrowseEvents();
						break;
					case "2":
						BuyTicket();
						break;
					case "3":
						MyTickets();
						break;
					case "4":
						CancelTicket();
						break;
					case "0":
						exit = true;
						break;
					default:
						System.Console.WriteLine("Invalid option.");
						Pause();
						break;
				}
			}
		}

		private void BrowseEvents()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== AVAILABLE EVENTS ===\n");

			var events = _eventService.GetAll();

			if (events.Count == 0)
			{
				System.Console.WriteLine("No events available.");
			}
			else
			{
				foreach (var ev in events)
				{
					System.Console.WriteLine($"[{ev.Id}] {ev.GetEventDetails()} | {ev.Date:dd.MM.yyyy} | {ev.Location}");
				}
			}

			Pause();
		}

		private void BuyTicket()
		{
			System.Console.Clear();
			System.Console.Write("Enter Event Id: ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int eventId))
			{
				System.Console.WriteLine("Invalid Event Id.");
				Pause();
				return;
			}

			System.Console.Write("Enter price: ");
			string priceInput = System.Console.ReadLine() ?? "";

			if (!decimal.TryParse(priceInput, out decimal price))
			{
				System.Console.WriteLine("Invalid price.");
				Pause();
				return;
			}

			try
			{
				_ticketService.BuyTicket(_currentUser.Id, eventId, price);
				System.Console.WriteLine("\nTicket purchased successfully!");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"\nError: {ex.Message}");
			}

			Pause();
		}

		private void MyTickets()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== MY TICKETS ===\n");

			var tickets = _ticketService.GetTicketsByUser(_currentUser.Id);

			if (tickets.Count == 0)
			{
				System.Console.WriteLine("You have no tickets.");
			}
			else
			{
				foreach (var t in tickets)
				{
					System.Console.WriteLine($"[{t.Id}] Event Id: {t.EventId} | Price: {t.Price} | Status: {t.Status} | Purchased: {t.PurchaseDate:dd.MM.yyyy}");
				}
			}

			Pause();
		}

		private void CancelTicket()
		{
			System.Console.Clear();
			System.Console.Write("Enter Ticket Id to cancel: ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int ticketId))
			{
				System.Console.WriteLine("Invalid Ticket Id.");
				Pause();
				return;
			}

			try
			{
				_ticketService.CancelTicket(ticketId);
				System.Console.WriteLine("\nTicket cancelled successfully!");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"\nError: {ex.Message}");
			}

			Pause();
		}

		private void Pause()
		{
			System.Console.WriteLine("\nPress any key to continue...");
			System.Console.ReadKey();
		}
	}
}