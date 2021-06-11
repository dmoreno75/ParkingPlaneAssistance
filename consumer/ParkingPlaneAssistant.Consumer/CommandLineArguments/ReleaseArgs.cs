using System;
using System.Collections.Generic;

using CommandLine;

using ParkingPlaneAssistant.Consumer.Commands;

namespace ParkingPlaneAssistant.Consumer
{
	[Verb("release", HelpText = "Make a available an slot previously booked given its booking identifer.")]
    public class ReleaseArgs: ArgsBase
	{
		[Option('i', "identifier", Required = true)]
		public string Identifier { get; set; }

		public override string Route => "release";
		public override IDictionary<string, string> GetQueryParameters()
		{
			return new Dictionary<string, string>()
			{
				{"bookingId", this.Identifier },
			};
		}
	}
}
