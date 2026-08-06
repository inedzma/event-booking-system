using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public static class EventFormatter
	{
		public delegate string FormatDelegate(Event ev);

		public static string ShortFormat(Event ev)
			=> $"{ev.Title} — {ev.Date:dd.MM.yyyy}";

		public static string DetailedFormat(Event ev)
			=> $"[{ev.Id}] {ev.GetEventDetails()} | Date: {ev.Date:dd.MM.yyyy} | Location: {ev.Location} | Capacity: {ev.Capacity}";
	}
}