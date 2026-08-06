using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;

namespace EventBookingSystem.Core.Services
{
	public class EventService : IEventService
	{
		private readonly IEventRepository _eventRepository;

		public EventService(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
		}

		private void ValidateCommon(string title, DateTime date, string location, int capacity)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Naziv eventa ne smije biti prazan.");

			if (string.IsNullOrWhiteSpace(location))
				throw new ArgumentException("Lokacija ne smije biti prazna.");

			if (capacity <= 0)
				throw new ArgumentException("Kapacitet mora biti veći od nule.");

			if (date < DateTime.Now)
				throw new ArgumentException("Datum eventa ne može biti u prošlosti.");
		}

		public Concert CreateConcert(int organizerId, string title, DateTime date, string location, int capacity, string performer)
		{
			ValidateCommon(title, date, location, capacity);

			if (string.IsNullOrWhiteSpace(performer))
				throw new ArgumentException("Izvođač mora biti unesen.");

			var concert = new Concert
			{
				OrganizerId = organizerId,
				Title = title,
				Date = date,
				Location = location,
				Capacity = capacity,
				Performer = performer
			};

			_eventRepository.Add(concert);
			return concert;
		}

		public Conference CreateConference(int organizerId, string title, DateTime date, string location, int capacity, string topic, int numberOfSpeakers)
		{
			ValidateCommon(title, date, location, capacity);

			if (string.IsNullOrWhiteSpace(topic))
				throw new ArgumentException("Tema konferencije mora biti unesena.");

			if (numberOfSpeakers <= 0)
				throw new ArgumentException("Broj govornika mora biti veći od nule.");

			var conference = new Conference
			{
				OrganizerId = organizerId,
				Title = title,
				Date = date,
				Location = location,
				Capacity = capacity,
				Topic = topic,
				NumberOfSpeakers = numberOfSpeakers
			};

			_eventRepository.Add(conference);
			return conference;
		}

		public Workshop CreateWorkshop(int organizerId, string title, DateTime date, string location, int capacity, int maxParticipantsPerGroup)
		{
			ValidateCommon(title, date, location, capacity);

			if (maxParticipantsPerGroup <= 0)
				throw new ArgumentException("Max broj učesnika po grupi mora biti veći od nule.");

			var workshop = new Workshop
			{
				OrganizerId = organizerId,
				Title = title,
				Date = date,
				Location = location,
				Capacity = capacity,
				MaxParticipantsPerGroup = maxParticipantsPerGroup
			};

			_eventRepository.Add(workshop);
			return workshop;
		}

		public List<Event> GetAll()
		{
			return _eventRepository.GetAll();
		}

		public Event GetById(int id)
		{
			return _eventRepository.GetById(id)
				?? throw new KeyNotFoundException($"Event sa Id {id} ne postoji.");
		}

		public void Delete(int id)
		{
			_eventRepository.Delete(id);
		}

		public bool IsDateAvailable(DateTime date, string location)
		{
			return !_eventRepository.GetAll()
				.Any(e => e.Date.Date == date.Date &&
						  e.Location.Equals(location, StringComparison.OrdinalIgnoreCase));
		}
	}


}