using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Core.Models
{
	public class Concert : Event
	{
		public string Performer { get; set; } = string.Empty;

		public override string GetEventDetails()
			=> $"Concert '{Title}' — performer: {Performer}";
		public override List<TicketOption> GetTicketOptions() => new()
		{
			new TicketOption("Fan Zone", 20),
			new TicketOption("Tribina", 35),
			new TicketOption("Parter", 50),
			new TicketOption("VIP", 80)
		};
	}

}
