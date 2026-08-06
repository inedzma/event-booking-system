using EventBookingSystem.Core.Models;

namespace EventBookingSystem.Core.Services
{
	public interface IEventService
	{
		Concert CreateConcert(int organizerId, string title, DateTime date, string location, int capacity, string performer);
		Conference CreateConference(int organizerId, string title, DateTime date, string location, int capacity, string topic, int numberOfSpeakers);
		Workshop CreateWorkshop(int organizerId, string title, DateTime date, string location, int capacity, int maxParticipantsPerGroup);
		List<Event> GetAll();
		Event GetById(int id);
		void Delete(int id);
		bool IsDateAvailable(DateTime date, string location);
	}
}