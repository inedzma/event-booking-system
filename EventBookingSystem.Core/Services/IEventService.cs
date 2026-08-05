using EventBookingSystem.Core.Models;
using System.Collections.Generic;

namespace EventBookingSystem.Core.Services
{
	public interface IEventService
	{
		Concert CreateConcert(string title, DateTime date, string location, int capacity, string performer);
		Conference CreateConference(string title, DateTime date, string location, int capacity, string topic, int numberOfSpeakers);
		Workshop CreateWorkshop(string title, DateTime date, string location, int capacity, int maxParticipantsPerGroup);
		List<Event> GetAll();
		Event GetById(int id);
		void Delete(int id);
	}
}