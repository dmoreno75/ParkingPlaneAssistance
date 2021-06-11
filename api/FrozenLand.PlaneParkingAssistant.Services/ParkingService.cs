using System;
using System.Linq;
using System.Collections.Generic;
using FrozenLand.PlaneParkingAssistant.Core.Interfaces;
using FrozenLand.PlaneParkingAssistant.Core.Helpers;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public class ParkingService : IParkingPlaneAssistant
	{
		private readonly IParkingPlaneAssistant _assistant;
		public ParkingService()
		{
			var mappingRepository = new PlaneModelTypesMappingRepository(SettingsConfigHelper.PlaneModelTypesMapping());
			var configuration = SettingsConfigHelper.Configuration();

			_assistant = new ParkingPlaneAssistant(configuration, mappingRepository);

		}
		public SlotOcuppancy Book(string model, DateTimeRange dateTimeRange)
		{
			return _assistant.Book(model, dateTimeRange);
		}

		public Slot Recommend(string model, DateTimeRange dateTimeRange)
		{
			return _assistant.Recommend(model, dateTimeRange);
		}

		public bool Release(string bookingId)
		{
			return _assistant.Release(bookingId);
		}

		public Slot Status(int slotNumber)
		{
			return _assistant.Status(slotNumber);
		}
	}
}
