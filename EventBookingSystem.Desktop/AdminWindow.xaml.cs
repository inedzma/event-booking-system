using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class AdminWindow : Window
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;
		private readonly IBookingService _bookingService;

		public AdminWindow(User currentUser)
		{
			InitializeComponent();
			_currentUser = currentUser;
			_eventService = App.Services.GetRequiredService<IEventService>();
			_bookingService = App.Services.GetRequiredService<IBookingService>();

			WelcomeText.Text = $"Welcome Admin {_currentUser.Name}!";
			LoadEvents();
		}

		private void LoadEvents()
		{
			EventsListBox.Items.Clear();
			var events = _eventService.GetAll();

			if (events.Count == 0)
			{
				EventsListBox.Items.Add("No events found.");
				return;
			}

			foreach (var ev in events)
			{
				EventsListBox.Items.Add($"[{ev.Id}] {ev.GetEventDetails()} | {ev.Date:dd.MM.yyyy} | {ev.Location} | Capacity: {ev.Capacity}");
			}
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			LoadEvents();
		}

		private void CreateEventButton_Click(object sender, RoutedEventArgs e)
		{
			var createWindow = new CreateEventWindow(_currentUser, _eventService);
			createWindow.ShowDialog();
			LoadEvents();
		}

		private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
		{
			string input = Microsoft.VisualBasic.Interaction.InputBox(
				"Enter Event Id to delete:", "Delete Event", "");

			if (string.IsNullOrWhiteSpace(input))
				return;

			if (!int.TryParse(input, out int id))
			{
				MessageBox.Show("Invalid Id.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
				return;
			}

			try
			{
				_eventService.Delete(id);
				MessageBox.Show("Event deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				LoadEvents();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void PendingBookingsButton_Click(object sender, RoutedEventArgs e)
		{
			var bookingsWindow = new PendingBookingsWindow(_eventService, _bookingService);
			bookingsWindow.ShowDialog();
			LoadEvents();
		}

		private void ReportsButton_Click(object sender, RoutedEventArgs e)
		{
			var reportsWindow = new ReportsWindow();
			reportsWindow.Show();
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}
	}
}