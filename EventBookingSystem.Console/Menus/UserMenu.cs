using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class UserMenu
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;
		private readonly ITicketService _ticketService;
		private readonly IBookingService _bookingService;

		public UserMenu(User currentUser, IEventService eventService, ITicketService ticketService, IBookingService bookingService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
			_ticketService = ticketService;
			_bookingService = bookingService;
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
				System.Console.WriteLine("5. Request Event Booking");
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
					case "5":
						RequestBooking();
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
			var tickets = _ticketService.GetAllValidTickets();

			if (events.Count == 0)
			{
				System.Console.WriteLine("No events available.");
				Pause();
				return;
			}

			foreach (var ev in events)
			{
				int sold = tickets.Count(t => t.EventId == ev.Id);
				int remaining = ev.Capacity - sold;
				System.Console.WriteLine($"[{ev.Id}] {ev.GetEventDetails()} | {ev.Date:dd.MM.yyyy} | {ev.Location} | Seats left: {remaining}/{ev.Capacity}");
			}

			System.Console.Write("\nWould you like to buy a ticket now? (y/n): ");
			string answer = (System.Console.ReadLine() ?? "").ToLower();

			if (answer == "y")
			{
				BuyTicket();
			}
		}

		private void BuyTicket()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== AVAILABLE EVENTS ===\n");

			var events = _eventService.GetAll();

			if (events.Count == 0)
			{
				System.Console.WriteLine("No events available.");
				Pause();
				return;
			}

			foreach (var e in events)
			{
				System.Console.WriteLine($"[{e.Id}] {e.Title} — {e.Date:dd.MM.yyyy}");
			}

			System.Console.Write("\nEnter Event Id: ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int eventId))
			{
				System.Console.WriteLine("Invalid Event Id.");
				Pause();
				return;
			}

			Event ev;
			try
			{
				ev = _eventService.GetById(eventId);
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"\nError: {ex.Message}");
				Pause();
				return;
			}

			var options = ev.GetTicketOptions();

			System.Console.WriteLine($"\nAvailable ticket categories for '{ev.Title}':");
			foreach (var option in options)
			{
				System.Console.WriteLine($"- {option.Name}: {option.Price} KM");
			}

			System.Console.Write("\nEnter category name: ");
			string category = System.Console.ReadLine() ?? "";

			try
			{
				var ticket = _ticketService.BuyTicket(eventId, _currentUser.Id, category);
				System.Console.WriteLine($"\nTicket purchased successfully! Category: {ticket.Category}, Price: {ticket.Price} KM");
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

			var tickets = _ticketService.GetUserTickets(_currentUser.Id);

			if (tickets.Count == 0)
			{
				System.Console.WriteLine("You have no tickets.");
			}
			else
			{
				foreach (var t in tickets)
				{
					System.Console.WriteLine($"[{t.Id}] Event Id: {t.EventId} | Category: {t.Category} | Price: {t.Price:F2} | Status: {t.Status} | Purchased: {t.PurchaseDate:dd.MM.yyyy}");
				}
			}

			Pause();
		}

		private void CancelTicket()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== MY TICKETS ===\n");

			var tickets = _ticketService.GetUserTickets(_currentUser.Id)
				.Where(t => t.Status == Core.Enums.TicketStatus.Valid)
				.ToList();

			if (tickets.Count == 0)
			{
				System.Console.WriteLine("You have no active tickets to cancel.");
				Pause();
				return;
			}

			foreach (var t in tickets)
			{
				System.Console.WriteLine($"[{t.Id}] Event Id: {t.EventId} | Category: {t.Category} | Price: {t.Price:F2} KM | Purchased: {t.PurchaseDate:dd.MM.yyyy}");
			}

			System.Console.Write("\nEnter Ticket Id to cancel: ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int ticketId))
			{
				System.Console.WriteLine("Invalid Ticket Id.");
				Pause();
				return;
			}

			try
			{
				_ticketService.CancelTicket(ticketId, _currentUser.Id);
				System.Console.WriteLine("\nTicket cancelled successfully!");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"\nError: {ex.Message}");
			}

			Pause();
		}

		private void RequestBooking()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== REQUEST EVENT BOOKING ===\n");

			System.Console.Write("Title: ");
			string title = System.Console.ReadLine() ?? "";

			System.Console.WriteLine("Category: 1. Concert  2. Conference  3. Workshop");
			System.Console.Write("Choose category: ");
			string? categoryInput = System.Console.ReadLine();

			EventCategory category = categoryInput switch
			{
				"1" => EventCategory.Concert,
				"2" => EventCategory.Conference,
				"3" => EventCategory.Workshop,
				_ => throw new ArgumentException("Nepoznata kategorija.")
			};

			System.Console.Write("Location: ");
			string location = System.Console.ReadLine() ?? "";

			System.Console.Write("Capacity: ");
			if (!int.TryParse(System.Console.ReadLine(), out int capacity))
			{
				System.Console.WriteLine("Invalid capacity.");
				Pause();
				return;
			}

			bool booked = false;

			while (!booked)
			{
				System.Console.Write("\nDate (dd.MM.yyyy): ");
				string dateInput = System.Console.ReadLine() ?? "";

				if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", null,
						System.Globalization.DateTimeStyles.None, out DateTime date))
				{
					System.Console.WriteLine("Invalid date format. Try again.");
					continue;
				}

				if (!_eventService.IsDateAvailable(date, location))
				{
					System.Console.WriteLine("That date is not available at this location. Please choose another date.");
					continue;
				}

				try
				{
					_bookingService.RequestBooking(_currentUser.Id, title, category, date, location, capacity);
					System.Console.WriteLine("\nDate is available! Booking request submitted for admin approval.");
					booked = true;
				}
				catch (Exception ex)
				{
					System.Console.WriteLine($"\nError: {ex.Message}");
					Pause();
					return;
				}
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