using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public interface IReportService
	{
		List<Event> SearchByTitle(string keyword);
		List<Event> FilterByDaysFromNow(string daysInput);
		Dictionary<string, int> GroupEventsByCategory();
		Dictionary<string, int> GroupTicketsByEvent();
		List<(Event Event, decimal TotalRevenue)> RevenueReport();
		Event? MostPopularEvent();
	}
}