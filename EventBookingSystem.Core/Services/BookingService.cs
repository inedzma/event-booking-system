using EventBookingSystem.Core.Enums;
using EventBookingSystem.Core.Models;
using EventBookingSystem.Core.Repositories;

namespace EventBookingSystem.Core.Services
{
	public class BookingService : IBookingService
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly IEventService _eventService;

		public BookingService(IBookingRepository bookingRepository, IEventService eventService)
		{
			_bookingRepository = bookingRepository;
			_eventService = eventService;
		}

		public Booking RequestBooking(int userId, string title, EventCategory category, DateTime date, string location, int capacity)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("Naziv ne smije biti prazan.");

			if (string.IsNullOrWhiteSpace(location))
				throw new ArgumentException("Lokacija ne smije biti prazna.");

			if (capacity <= 0)
				throw new ArgumentException("Kapacitet mora biti veći od nule.");

			if (date < DateTime.Now)
				throw new ArgumentException("Datum ne može biti u prošlosti.");

			if (!_eventService.IsDateAvailable(date, location))
				throw new InvalidOperationException("Termin je zauzet na toj lokaciji. Izaberite drugi datum ili lokaciju.");

			var booking = new Booking
			{
				UserId = userId,
				ProposedTitle = title,
				Category = category,
				ProposedDate = date,
				ProposedLocation = location,
				ProposedCapacity = capacity,
				Status = BookingStatus.Pending,
				RequestDate = DateTime.Now
			};

			_bookingRepository.Add(booking);
			return booking;
		}

		public List<Booking> GetPending()
		{
			return _bookingRepository.GetAll()
				.Where(b => b.Status == BookingStatus.Pending)
				.ToList();
		}

		public List<Booking> GetAll()
		{
			return _bookingRepository.GetAll();
		}

		public void Reject(int bookingId)
		{
			var booking = _bookingRepository.GetById(bookingId)
				?? throw new KeyNotFoundException("Zahtjev ne postoji.");

			booking.Status = BookingStatus.Rejected;
			_bookingRepository.Update(booking);
		}

		public Event Approve(int bookingId, string? performer, string? topic, int? numberOfSpeakers, int? maxParticipantsPerGroup)
		{
			var booking = _bookingRepository.GetById(bookingId)
				?? throw new KeyNotFoundException("Zahtjev ne postoji.");

			Event createdEvent = booking.Category switch
			{
				EventCategory.Concert => _eventService.CreateConcert(
					booking.UserId, booking.ProposedTitle, booking.ProposedDate,
					booking.ProposedLocation, booking.ProposedCapacity, performer ?? "TBA"),

				EventCategory.Conference => _eventService.CreateConference(
					booking.UserId, booking.ProposedTitle, booking.ProposedDate,
					booking.ProposedLocation, booking.ProposedCapacity, topic ?? "TBA", numberOfSpeakers ?? 1),

				EventCategory.Workshop => _eventService.CreateWorkshop(
					booking.UserId, booking.ProposedTitle, booking.ProposedDate,
					booking.ProposedLocation, booking.ProposedCapacity, maxParticipantsPerGroup ?? 5),

				_ => throw new InvalidOperationException("Nepoznat tip eventa.")
			};

			booking.Status = BookingStatus.Approved;
			_bookingRepository.Update(booking);

			return createdEvent;
		}
	}
}