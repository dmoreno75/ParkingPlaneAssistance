using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ParkingPlaneAssistant.Consumer.Commands;

namespace ParkingPlaneAssistant.Consumer
{
	public abstract class ArgsBase
	{
		protected readonly string baseUrl;

		public ArgsBase()
		{
			var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false)
			.Build();

			baseUrl = configuration.GetValue<string>("ParkingPlaneAssistant:BaseUrl");
		}

		public abstract string Route { get; }
		public abstract IDictionary<string, string> GetQueryParameters();

		public async Task<APIResponse> Execute()
		{
			if (string.IsNullOrEmpty(baseUrl)) 
				throw new Exception("Couldn't find BaseUrl on appsetting.json file");

			var request = new APIRequests(baseUrl, Route);

			var query = this.GetQueryParameters();

			return await request.Run(query);
		}
	}
}
