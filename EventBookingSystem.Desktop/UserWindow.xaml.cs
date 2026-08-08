using EventBookingSystem.Core.Models;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class UserWindow : Window
	{
		private readonly User _currentUser;

		public UserWindow(User currentUser)
		{
			InitializeComponent();
			_currentUser = currentUser;
			Title = $"User - {currentUser.Name}";
		}
	}
}