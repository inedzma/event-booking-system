using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class PendingBookingsWindow : Window
	{
		private readonly IEventService _eventService;
		private readonly IBookingService _bookingService;
		private List<Booking> _pendingBookings = new();

		public PendingBookingsWindow(IEventService eventService, IBookingService bookingService)
		{
			InitializeComponent();
			_eventService = eventService;
			_bookingService = bookingService;
			LoadBookings();
		}

		private void LoadBookings()
		{
			BookingsListBox.Items.Clear();
			_pendingBookings = _bookingService.GetPending();

			if (_pendingBookings.Count == 0)
			{
				BookingsListBox.Items.Add("No pending booking requests.");
				return;
			}

			foreach (var b in _pendingBookings)
			{
				BookingsListBox.Items.Add($"[{b.Id}] {b.ProposedTitle} ({b.Category}) | {b.ProposedDate:dd.MM.yyyy} | {b.ProposedLocation} | Capacity: {b.ProposedCapacity} | UserId: {b.UserId}");
			}
		}

		private void RefreshButton_Click(object sender, RoutedEventArgs e)
		{
			LoadBookings();
		}

		private Booking? GetSelectedBooking()
		{
			if (BookingsListBox.SelectedIndex < 0 || _pendingBookings.Count == 0)
			{
				MessageBox.Show("Please select a booking from the list.", "No selection", MessageBoxButton.OK, MessageBoxImage.Warning);
				return null;
			}

			return _pendingBookings[BookingsListBox.SelectedIndex];
		}

		private void ApproveButton_Click(object sender, RoutedEventArgs e)
		{
			var booking = GetSelectedBooking();
			if (booking == null) return;

			string? performer = null;
			string? topic = null;
			int? speakers = null;
			int? maxGroup = null;

			string extra = Microsoft.VisualBasic.Interaction.InputBox(
				$"Enter extra info for {booking.Category}:\n(Performer / Topic / Max group size)",
				"Approve Booking", "");

			switch (booking.Category)
			{
				case EventCategory.Concert:
					performer = extra;
					break;
				case EventCategory.Conference:
					topic = extra;
					speakers = 1;
					break;
				case EventCategory.Workshop:
					maxGroup = int.TryParse(extra, out int mg) ? mg : 5;
					break;
			}

			try
			{
				_bookingService.Approve(booking.Id, performer, topic, speakers, maxGroup);
				MessageBox.Show("Booking approved and event created!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				LoadBookings();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void RejectButton_Click(object sender, RoutedEventArgs e)
		{
			var booking = GetSelectedBooking();
			if (booking == null) return;

			try
			{
				_bookingService.Reject(booking.Id);
				MessageBox.Show("Booking rejected.", "Done", MessageBoxButton.OK, MessageBoxImage.Information);
				LoadBookings();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}