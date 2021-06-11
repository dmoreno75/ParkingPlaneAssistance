using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParkingPlaneAssistant.Consumer
{
	[Verb("book", HelpText = "Reserve an slot given a model and dates. Will returns a booking identifier.")]
    public class BookArgs: ArgsBase
    {
		[Option('m', "model", Required = true)]
        public string Model { get; set; }

        [Option('s', "start", Required = true)]
        public DateTime Start { get; set; }

        [Option('e', "end", Required = true)]
        public DateTime End { get; set; }

		public override string Route => "book";

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
