using System;
using System.Collections.Generic;

using CommandLine;

using ParkingPlaneAssistant.Consumer.Commands;

namespace ParkingPlaneAssistant.Consumer
{
	[Verb("recommend", HelpText = "Recommends an empty slot given a plane model.")]
    public class RecommendsArgs: ArgsBase
	{
		[Option('m', "model", Required = true)]
		public string Model { get; set; }

		[Option('s', "start", Required = true)]
		public DateTime Start { get; set; }

		[Option('e', "end", Required = true)]
		public DateTime End { get; set; }
		public override string Route => "recommend";
		public override IDictionary<string, string> GetQueryParameters()
		{
			return new Dictionary<string, string>()
			{
				{"model", this.Model },
				{"start", this.Start.ToString() },
				{"end", this.End.ToString() }
			};
		}
	}
}
