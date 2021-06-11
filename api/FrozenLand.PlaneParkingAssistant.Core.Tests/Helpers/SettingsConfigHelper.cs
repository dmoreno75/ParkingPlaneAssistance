using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FrozenLand.PlaneParkingAssistant.Core.Tests.Helpers
{
	public class SettingsConfigHelper
	{
		private static SettingsConfigHelper _appSettings;

		public string appSettingValue { get; set; }

		public static string AppSetting(string Key)
		{
			_appSettings = GetCurrentSettings(Key);

			return _appSettings.appSettingValue;
		}

		public SettingsConfigHelper(IConfiguration config, string Key)
		{
			this.appSettingValue = config.GetValue<string>(Key);
		}

		// Get a valued stored in the appsettings.
		// Pass in a key like TestArea:TestKey to get TestValue
		public static SettingsConfigHelper GetCurrentSettings(string Key)
		{
			var configuration = GetConfigurationRoot();
			var settings = new SettingsConfigHelper(configuration.GetSection("ApplicationSettings"), Key);

			return settings;
		}

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

		private static IConfigurationRoot GetConfigurationRoot()
		{
			var builder = new ConfigurationBuilder()
							.SetBasePath(Directory.GetCurrentDirectory())
							.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
							.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}
