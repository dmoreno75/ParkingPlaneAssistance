using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParkingPlaneAssistant.Consumer
{
	[Verb("status", HelpText = "Displays the bookings for a given slot number.")]
    public class StatusArgs : ArgsBase
    {
		[Option('s', "slot-number", Required = true)]
        public string Number { get; set; }

		public override string Route => "status";

		public override IDictionary<string, string> GetQueryParameters()
		{
			return new Dictionary<string, string>()
			{
				{"slotNumber", this.Number },
			};
		}
	}
}
