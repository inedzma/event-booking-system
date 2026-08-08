using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;
using System.Windows;
using System.Windows.Media;

namespace EventBookingSystem.Desktop
{
	public partial class CreateEventWindow : Window
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;

		public CreateEventWindow(User currentUser, IEventService eventService)
		{
			InitializeComponent();
			_currentUser = currentUser;
			_eventService = eventService;
		}

		private void TypeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			// Fields nisu inicijalizovani prvi put dok se prozor učitava (event puca prije InitializeComponent završi)
			if (ConcertPanel == null || ConferencePanel == null || WorkshopPanel == null)
				return;

			ConcertPanel.Visibility = Visibility.Collapsed;
			ConferencePanel.Visibility = Visibility.Collapsed;
			WorkshopPanel.Visibility = Visibility.Collapsed;

			switch (TypeComboBox.SelectedIndex)
			{
				case 0: // Concert
					ConcertPanel.Visibility = Visibility.Visible;
					break;
				case 1: // Conference
					ConferencePanel.Visibility = Visibility.Visible;
					break;
				case 2: // Workshop
					WorkshopPanel.Visibility = Visibility.Visible;
					break;
			}
		}

		private void CreateButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				string title = TitleTextBox.Text;
				string location = LocationTextBox.Text;

				if (!DateTime.TryParseExact(DateTextBox.Text, "dd.MM.yyyy", null,
						System.Globalization.DateTimeStyles.None, out DateTime date))
				{
					ShowError("Invalid date format. Use dd.MM.yyyy.");
					return;
				}

				if (!int.TryParse(CapacityTextBox.Text, out int capacity))
				{
					ShowError("Invalid capacity.");
					return;
				}

				switch (TypeComboBox.SelectedIndex)
				{
					case 0: // Concert
						_eventService.CreateConcert(_currentUser.Id, title, date, location, capacity, PerformerTextBox.Text);
						break;

					case 1: // Conference
						if (!int.TryParse(SpeakersTextBox.Text, out int speakers))
						{
							ShowError("Invalid number of speakers.");
							return;
						}
						_eventService.CreateConference(_currentUser.Id, title, date, location, capacity, TopicTextBox.Text, speakers);
						break;

					case 2: // Workshop
						if (!int.TryParse(MaxGroupTextBox.Text, out int maxGroup))
						{
							ShowError("Invalid max participants per group.");
							return;
						}
						_eventService.CreateWorkshop(_currentUser.Id, title, date, location, capacity, maxGroup);
						break;
				}

				MessageText.Foreground = Brushes.Green;
				MessageText.Text = "Event created successfully!";
			}
			catch (Exception ex)
			{
				ShowError(ex.Message);
			}
		}

		private void ShowError(string message)
		{
			MessageText.Foreground = Brushes.Red;
			MessageText.Text = message;
		}
	}
}