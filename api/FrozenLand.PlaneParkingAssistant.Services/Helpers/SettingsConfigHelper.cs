using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrozenLand.PlaneParkingAssistant.Core.Helpers
{
	public class SettingsConfigHelper
	{
		public static IDictionary<string, PlaneType> PlaneModelTypesMapping()
		{
			var config = GetConfigurationRoot();
			var repo = new Dictionary<string, PlaneType>();

			var mappingSection = config.GetSection("ApplicationSettings:Mapping");

			mappingSection
				.GetSection("Props")
				.GetChildren()
				.ToList()
				.ForEach(x => repo.Add(x.Value, PlaneType.Props));

			mappingSection
				.GetSection("Jets")
				.GetChildren()
				.ToList()
				.ForEach(x => repo.Add(x.Value, PlaneType.Jets));

			mappingSection
				.GetSection("Jumbos")
				.GetChildren()
				.ToList()
				.ForEach(x => repo.Add(x.Value, PlaneType.Jumbos));


			return repo;
		}

		public static IList<SlotsConfiguration> Configuration()
		{
			var config = GetConfigurationRoot()
				.GetSection("ApplicationSettings:Parking"); ;

			var props = config.GetValue<int>("Props");
			var jets = config.GetValue<int>("Jets");
			var jumbos = config.GetValue<int>("Jumbos");

			return new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(props, PlaneType.Props),
				SlotsConfiguration.Builder(jets, PlaneType.Jets),
				SlotsConfiguration.Builder(jumbos, PlaneType.Jumbos)
			};
		}

		private static IConfigurationRoot GetConfigurationRoot()
		{
			var builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
							;

			return builder.Build();
		}
	}
}
