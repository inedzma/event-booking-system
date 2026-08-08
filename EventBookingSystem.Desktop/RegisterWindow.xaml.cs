using EventBookingSystem.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Media;

namespace EventBookingSystem.Desktop
{
	public partial class RegisterWindow : Window
	{
		private readonly IUserService _userService;

		public RegisterWindow()
		{
			InitializeComponent();
			_userService = App.Services.GetRequiredService<IUserService>();
		}

		private void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_userService.Register(NameTextBox.Text, EmailTextBox.Text, PasswordBox.Password);
				MessageText.Foreground = Brushes.Green;
				MessageText.Text = "Registration successful! You can now log in.";
			}
			catch (Exception ex)
			{
				MessageText.Foreground = Brushes.Red;
				MessageText.Text = ex.Message;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			var loginWindow = new LoginWindow();
			loginWindow.Show();
			Close();
		}
	}
}