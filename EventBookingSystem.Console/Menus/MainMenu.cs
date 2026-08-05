using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Services;

namespace EventBookingSystem.Console.Menus
{
	public class MainMenu
	{
		private readonly User _currentUser;
		private readonly IEventService _eventService;
		private readonly ITicketService _ticketService;

		public MainMenu(User currentUser, IEventService eventService, ITicketService ticketService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
			_ticketService = ticketService;
		}

		public void Run()
		{
			if (_currentUser.Role == UserRole.Admin)
			{
				var adminMenu = new AdminMenu(_currentUser, _eventService);
				adminMenu.Run();
			}
			else
			{
				var userMenu = new UserMenu(_currentUser, _eventService, _ticketService);
				userMenu.Run();
			}
		}
	}
}