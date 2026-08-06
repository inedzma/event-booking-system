using System;
using System.Collections.Generic;
using System.Text;

namespace EventBookingSystem.Core.Models
{
	public class Workshop : Event
	{
		public int MaxParticipantsPerGroup { get; set; }

		public override string GetEventDetails()
			=> $"Workshop '{Title}' — max group size: {MaxParticipantsPerGroup}";

		public override List<TicketOption> GetTicketOptions() => new()
		{
			new TicketOption("Standard", 25),
			new TicketOption("With Materials", 40)
		};
	}


}
