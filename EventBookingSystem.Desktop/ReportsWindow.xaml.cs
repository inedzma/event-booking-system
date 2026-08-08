using EventBookingSystem.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class ReportsWindow : Window
	{
		private readonly IReportService _reportService;

		public ReportsWindow()
		{
			InitializeComponent();
			_reportService = App.Services.GetRequiredService<IReportService>();
		}

		private void GroupByCategory_Click(object sender, RoutedEventArgs e)
		{
			ResultsListBox.Items.Clear();
			var grouped = _reportService.GroupEventsByCategory();

			if (grouped.Count == 0)
			{
				ResultsListBox.Items.Add("No events found.");
				return;
			}

			foreach (var kvp in grouped)
			{
				ResultsListBox.Items.Add($"{kvp.Key}: {kvp.Value} event(s)");
			}
		}

		private void RevenueReport_Click(object sender, RoutedEventArgs e)
		{
			ResultsListBox.Items.Clear();
			var report = _reportService.RevenueReport();

			if (report.Count == 0)
			{
				ResultsListBox.Items.Add("No events found.");
				return;
			}

			foreach (var (ev, revenue) in report)
			{
				ResultsListBox.Items.Add($"{ev.Title}: {revenue:F2} KM");
			}
		}

		private void MostPopular_Click(object sender, RoutedEventArgs e)
		{
			ResultsListBox.Items.Clear();
			var ev = _reportService.MostPopularEvent();

			ResultsListBox.Items.Add(ev == null
				? "No tickets sold yet."
				: EventFormatter.DetailedFormat(ev));
		}

		private void Availability_Click(object sender, RoutedEventArgs e)
		{
			ResultsListBox.Items.Clear();
			var report = _reportService.AvailabilityReport();

			if (report.Count == 0)
			{
				ResultsListBox.Items.Add("No events found.");
				return;
			}

			foreach (var (ev, sold, remaining) in report)
			{
				string status = remaining == 0 ? "SOLD OUT" : $"{remaining} seat(s) left";
				ResultsListBox.Items.Add($"{ev.Title} | Capacity: {ev.Capacity} | Sold: {sold} | {status}");
			}
		}
	}
}