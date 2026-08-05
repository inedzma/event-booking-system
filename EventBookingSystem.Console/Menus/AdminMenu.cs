using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class AdminMenu
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;

		public AdminMenu(User currentUser, IEventService eventService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
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
						_eventService.CreateConcert(title, date, location, capacity, performer);
						break;

					case "2":
						System.Console.Write("Topic: ");
						string topic = System.Console.ReadLine() ?? "";
						System.Console.Write("Number of speakers: ");
						int speakers = int.Parse(System.Console.ReadLine() ?? "");
						_eventService.CreateConference(title, date, location, capacity, topic, speakers);
						break;

					case "3":
						System.Console.Write("Max participants per group: ");
						int maxGroup = int.Parse(System.Console.ReadLine() ?? "");
						_eventService.CreateWorkshop(title, date, location, capacity, maxGroup);
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

		private void Pause()
		{
			System.Console.WriteLine("\nPress any key to continue...");
			System.Console.ReadKey();
		}
	}
}