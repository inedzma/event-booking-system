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
	}
}
