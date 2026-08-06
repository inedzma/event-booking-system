using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class ReportMenu
	{
		private readonly IReportService _reportService;

		public ReportMenu(IReportService reportService)
		{
			_reportService = reportService;
		}

		public void Run()
		{
			bool back = false;

			while (!back)
			{
				System.Console.Clear();
				System.Console.WriteLine("=== REPORTS & SEARCH ===\n");
				System.Console.WriteLine("1. Search events by title");
				System.Console.WriteLine("2. Filter events (next X days)");
				System.Console.WriteLine("3. Group events by category");
				System.Console.WriteLine("4. Group tickets by event");
				System.Console.WriteLine("5. Revenue report");
				System.Console.WriteLine("6. Most popular event");
				System.Console.WriteLine("0. Back");
				System.Console.Write("\nChoose an option: ");

				string? choice = System.Console.ReadLine();

				switch (choice)
				{
					case "1": SearchByTitle(); break;
					case "2": FilterByDays(); break;
					case "3": GroupByCategory(); break;
					case "4": GroupTicketsByEvent(); break;
					case "5": RevenueReport(); break;
					case "6": MostPopular(); break;
					case "0": back = true; break;
					default:
						System.Console.WriteLine("Invalid option.");
						Pause();
						break;
				}
			}
		}

		private void SearchByTitle()
		{
			System.Console.Clear();
			System.Console.Write("Enter search keyword: ");
			string keyword = System.Console.ReadLine() ?? "";

			try
			{
				var results = _reportService.SearchByTitle(keyword);
				PrintEvents(results, "No events matched your search.");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error: {ex.Message}");
			}

			Pause();
		}

		private void FilterByDays()
		{
			System.Console.Clear();
			System.Console.Write("Show events happening in the next how many days? ");
			string daysInput = System.Console.ReadLine() ?? "";

			try
			{
				var results = _reportService.FilterByDaysFromNow(daysInput);
				PrintEvents(results, $"No events happening in the next {daysInput} days.");
			}
			catch (Exception ex)
			{
				System.Console.WriteLine($"Error: {ex.Message}");
			}

			Pause();
		}

		private void GroupByCategory()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== EVENTS BY CATEGORY ===\n");

			var grouped = _reportService.GroupEventsByCategory();

			if (grouped.Count == 0)
			{
				System.Console.WriteLine("No events found.");
			}
			else
			{
				foreach (var kvp in grouped)
				{
					System.Console.WriteLine($"{kvp.Key}: {kvp.Value} event(s)");
				}
			}

			Pause();
		}

		private void GroupTicketsByEvent()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== TICKETS SOLD PER EVENT ===\n");

			var grouped = _reportService.GroupTicketsByEvent();

			if (grouped.Count == 0)
			{
				System.Console.WriteLine("No tickets sold yet.");
			}
			else
			{
				foreach (var kvp in grouped)
				{
					System.Console.WriteLine($"{kvp.Key}: {kvp.Value} ticket(s)");
				}
			}

			Pause();
		}

		private void RevenueReport()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== REVENUE REPORT ===\n");

			var report = _reportService.RevenueReport();

			if (report.Count == 0)
			{
				System.Console.WriteLine("No events found.");
			}
			else
			{
				foreach (var (ev, revenue) in report)
				{
					System.Console.WriteLine($"{ev.Title}: {revenue} KM");
				}
			}

			Pause();
		}

		private void MostPopular()
		{
			System.Console.Clear();
			System.Console.WriteLine("=== MOST POPULAR EVENT ===\n");

			var ev = _reportService.MostPopularEvent();

			if (ev == null)
			{
				System.Console.WriteLine("No tickets sold yet.");
			}
			else
			{
				System.Console.WriteLine(EventFormatter.DetailedFormat(ev));
			}

			Pause();
		}

		private void PrintEvents(List<Core.Models.Event> events, string emptyMessage)
		{
			if (events.Count == 0)
			{
				System.Console.WriteLine(emptyMessage);
				return;
			}

			foreach (var ev in events)
			{
				System.Console.WriteLine(EventFormatter.DetailedFormat(ev));
			}
		}

		private void Pause()
		{
			System.Console.WriteLine("\nPress any key to continue...");
			System.Console.ReadKey();
		}
	}
}