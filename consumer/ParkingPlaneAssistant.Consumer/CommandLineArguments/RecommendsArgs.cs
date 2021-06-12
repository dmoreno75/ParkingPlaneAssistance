using System;
using System.Collections.Generic;

using CommandLine;

namespace ParkingPlaneAssistant.Consumer
{
	[Verb("recommend", HelpText = "Recommends an empty slot given a plane model.")]
    public class RecommendsArgs: ArgsBase
	{
		[Option('m', "model", Required = true)]
		public string Model { get; set; }

		public override string Route => "recommend";
		public override IDictionary<string, string> GetQueryParameters()
		{
			return new Dictionary<string, string>()
			{
				{"model", this.Model }
			};
		}
	}
}
