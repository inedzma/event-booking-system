using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Core.Models
{


	public class Conference : Event
	{
		public string Topic { get; set; } = string.Empty;
		public int NumberOfSpeakers { get; set; }

		public override string GetEventDetails()
			=> $"Conference '{Title}' — topic: {Topic}, speakers: {NumberOfSpeakers}";

		public override List<TicketOption> GetTicketOptions() => new()
		{
			new TicketOption("General", 30),
			new TicketOption("Premium", 60),
			new TicketOption("VIP", 100)
		};
	}
}
