using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace EventBookingSystem.Desktop
{
	public partial class LoginWindow : Window
	{
		private readonly IUserService _userService;

		public LoginWindow()
		{
			InitializeComponent();
			_userService = App.Services.GetRequiredService<IUserService>();
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			string password = PasswordBox.Password;

			var user = _userService.Login(email, password);

			if (user == null)
			{
				ErrorText.Text = "Invalid email or password.";
				return;
			}

			OpenMainWindow(user);
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			var registerWindow = new RegisterWindow();
			registerWindow.Show();
			Close();
		}

		private void OpenMainWindow(EventBookingSystem.Core.Models.User user)
		{
			if (user.Role == UserRole.Admin)
			{
				var adminWindow = new AdminWindow(user);
				adminWindow.Show();
			}
			else
			{
				var userWindow = new UserWindow(user);
				userWindow.Show();
			}

			Close();
		}
	}
}