using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class AdminMenu
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;
		private readonly IBookingService _bookingService;

		public AdminMenu(User currentUser, IEventService eventService, IBookingService bookingService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
			_bookingService = bookingService;
		}

		public void Run()
		{
			bool exit = false;

			while (!exit)
			{
				System.Console.Clear();
				System.Console.WriteLine($"Welcome Admin {_currentUser.Name}!\n");
				System.Console.WriteLine("1. Create Event");
				System.Console.WriteLine("2. View All Events");
				System.Console.WriteLine("3. Delete Event");
				System.Console.WriteLine("4. View Pending Bookings");
				System.Console.WriteLine("0. Logout");
				System.Console.Write("\nChoose an option: ");

				string? choice = System.Console.ReadLine();

				switch (choice)
				{
					case "1":
						CreateEvent();
						break;
					case "2":
						ViewAllEvents();
						break;
					case "3":
						DeleteEvent();
						break;
					case "4":
						ViewPendingBookings();
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

		private void CreateEvent()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== CREATE EVENT ===");
			System.Console.WriteLine("1. Concert");
			System.Console.WriteLine("2. Conference");
			System.Console.WriteLine("3. Workshop");
			System.Console.Write("Choose event type: ");
			string? type = System.Console.ReadLine();

			try
			{
				System.Console.Write("Title: ");
				string title = System.Console.ReadLine() ?? "";

				System.Console.Write("Date (dd.MM.yyyy): ");
				string dateInput = System.Console.ReadLine() ?? "";
				DateTime date = DateTime.ParseExact(dateInput, "dd.MM.yyyy", null);

				System.Console.Write("Location: ");
				string location = System.Console.ReadLine() ?? "";

				System.Console.Write("Capacity: ");
				int capacity = int.Parse(System.Console.ReadLine() ?? "");

				switch (type)
				{
					case "1":
						System.Console.Write("Performer: ");
						string performer = System.Console.ReadLine() ?? "";
						_eventService.CreateConcert(_currentUser.Id, title, date, location, capacity, performer);
						break;

					case "2":
						System.Console.Write("Topic: ");
						string topic = System.Console.ReadLine() ?? "";
						System.Console.Write("Number of speakers: ");
						int speakers = int.Parse(System.Console.ReadLine() ?? "");
						_eventService.CreateConference(_currentUser.Id, title, date, location, capacity, topic, speakers);
						break;

					case "3":
						System.Console.Write("Max participants per group: ");
						int maxGroup = int.Parse(System.Console.ReadLine() ?? "");
						_eventService.CreateWorkshop(_currentUser.Id, title, date, location, capacity, maxGroup);
						break;

					default:
						System.Console.WriteLine("Invalid event type.");
						Pause();
						return;
				}

				System.Console.WriteLine("\nEvent created successfully!");
			}
			catch (FormatException)
			{
				System.Console.WriteLine("\nInvalid input format (check date or number fields).");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"\nError: {ex.Message}");
			}

			Pause();
		}

		private void ViewAllEvents()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== ALL EVENTS ===\n");

			var events = _eventService.GetAll();

			if (events.Count == 0)
			{
				System.Console.WriteLine("No events found.");
			}
			else
			{
				foreach (var ev in events)
				{
					System.Console.WriteLine($"[{ev.Id}] {ev.GetEventDetails()} | {ev.Date:dd.MM.yyyy} | {ev.Location} | Capacity: {ev.Capacity}");
				}
			}

			Pause();
		}

		private void DeleteEvent()
		{
			System.Console.Clear();
			System.Console.Write("Enter Event Id to delete: ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int id))
			{
				System.Console.WriteLine("Invalid Id.");
				Pause();
				return;
			}

			try
			{
				_eventService.Delete(id);
				System.Console.WriteLine("Event deleted successfully!");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error: {ex.Message}");
			}

			Pause();
		}

		private void ViewPendingBookings()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== PENDING BOOKINGS ===\n");

			var pending = _bookingService.GetPending();

			if (pending.Count == 0)
			{
				System.Console.WriteLine("No pending booking requests.");
				Pause();
				return;
			}

			foreach (var b in pending)
			{
				System.Console.WriteLine($"[{b.Id}] {b.ProposedTitle} ({b.Category}) | {b.ProposedDate:dd.MM.yyyy} | {b.ProposedLocation} | Capacity: {b.ProposedCapacity} | UserId: {b.UserId}");
			}

			System.Console.Write("\nEnter Booking Id to approve/reject (or 0 to go back): ");
			string input = System.Console.ReadLine() ?? "";

			if (!int.TryParse(input, out int bookingId) || bookingId == 0)
			{
				return;
			}

			System.Console.Write("Approve (A) or Reject (R)? ");
			string decision = (System.Console.ReadLine() ?? "").ToUpper();

			try
			{
				if (decision == "A")
				{
					var booking = pending.First(b => b.Id == bookingId);

					string? performer = null;
					string? topic = null;
					int? speakers = null;
					int? maxGroup = null;

					switch (booking.Category)
					{
						case EventCategory.Concert:
							System.Console.Write("Performer: ");
							performer = System.Console.ReadLine();
							break;
						case EventCategory.Conference:
							System.Console.Write("Topic: ");
							topic = System.Console.ReadLine();
							System.Console.Write("Number of speakers: ");
							speakers = int.Parse(System.Console.ReadLine() ?? "1");
							break;
						case EventCategory.Workshop:
							System.Console.Write("Max participants per group: ");
							maxGroup = int.Parse(System.Console.ReadLine() ?? "5");
							break;
					}

					_bookingService.Approve(bookingId, performer, topic, speakers, maxGroup);
					System.Console.WriteLine("\nBooking approved and event created!");
				}
				else if (decision == "R")
				{
					_bookingService.Reject(bookingId);
					System.Console.WriteLine("\nBooking rejected.");
				}
				else
				{
					System.Console.WriteLine("Invalid decision.");
				}
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