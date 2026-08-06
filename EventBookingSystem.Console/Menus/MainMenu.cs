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
		private readonly IBookingService _bookingService;

		public MainMenu(User currentUser, IEventService eventService, ITicketService ticketService, IBookingService bookingService)
		{
			_currentUser = currentUser;
			_eventService = eventService;
			_ticketService = ticketService;
			_bookingService = bookingService;
		}

		public void Run()
		{
			if (_currentUser.Role == UserRole.Admin)
			{
				var adminMenu = new AdminMenu(_currentUser, _eventService, _bookingService);
				adminMenu.Run();
			}
			else
			{
				var userMenu = new UserMenu(_currentUser, _eventService, _ticketService, _bookingService);
				userMenu.Run();
			}
		}
	}
}