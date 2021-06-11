using CommandLine;
using ParkingPlaneAssistant.Consumer.Commands;
using System;
using System.Threading.Tasks;

namespace ParkingPlaneAssistant.Consumer
{
	class Program
	{
		static void Main(string[] args)
		{
			APIResponse response = null;

			Parser.Default.ParseArguments<RecommendsArgs, BookArgs, ReleaseArgs, StatusArgs>(args)
			   .WithParsed<RecommendsArgs>(async o =>
			   {
				   response = await o.Execute();
			   })
			   .WithParsed<BookArgs>(async o =>
			   {
				   response = await o.Execute();
			   })
			   .WithParsed<ReleaseArgs>(async o =>
			   {
				   response = await o.Execute();
			   })
			   .WithParsed<StatusArgs>(async o =>
			   {
				   response = await o.Execute();
			   });

			Console.WriteLine(response?.Message);
		}
	}
}
