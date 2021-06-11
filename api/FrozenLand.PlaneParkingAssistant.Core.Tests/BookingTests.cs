using FrozenLand.PlaneParkingAssistant.Core.Tests.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace FrozenLand.PlaneParkingAssistant.Core.Tests
{
	public class BookingTests
	{
		private PlaneModelTypesMappingRepository mappingRepository;

		[SetUp]
		public void Setup()
		{
			mappingRepository = new PlaneModelTypesMappingRepository(SettingsConfigHelper.PlaneModelTypesMapping());
		}

		[Test]
		public void NotOverlapingDateTimeRange_WillReturnAvailableSlotIfthereIsAny()
		{
			var dateTimeRangeToday = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));
			var dateTimeRangeTomorrow = new DateTimeRange(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(1, PlaneType.Props),

			}, mappingRepository);

			var slotOcuppancy = parkingSpace.Book("E195", dateTimeRangeToday);
			Assert.IsNotNull(slotOcuppancy);
			//Assert.AreEqual(PlaneType.Props, slotOcuppancy.Type);

			var slotOcuppancy2 = parkingSpace.Book("E195", dateTimeRangeTomorrow);
			Assert.IsNotNull(slotOcuppancy2);
			//Assert.AreEqual(PlaneType.Props, slot2.Type);

			Assert.AreEqual(slotOcuppancy2.SlotNumber, slotOcuppancy2.SlotNumber);
		}


		[Test]
		public void OverlapingDateTimeRangeWithSingleParkingSlot_WontReturnAnySlot()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(1, PlaneType.Props),

			}, mappingRepository);

			var slotOcuppancy = parkingSpace.Book("E195", dateTimeRange);
			Assert.IsNotNull(slotOcuppancy);
			//Assert.AreEqual(PlaneType.Props, slot.Type);

			var slotOcuppancy2 = parkingSpace.Book("E195", dateTimeRange);
			Assert.IsNull(slotOcuppancy2);
		}

		[Test]
		public void OverlapingDateTimeRangeWithSeveralParkingSlot_WillReturnASlot()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(2, PlaneType.Props),

			}, mappingRepository);

			var slotOcuppancy = parkingSpace.Book("E195", dateTimeRange);
			Assert.IsNotNull(slotOcuppancy);
			//Assert.AreEqual(PlaneType.Props, slot.Type);

			var slotOcuppancy2 = parkingSpace.Book("E195", dateTimeRange);
			Assert.IsNotNull(slotOcuppancy2);
			//Assert.AreEqual(PlaneType.Props, slot2.Type);

			Assert.AreNotEqual(slotOcuppancy.SlotNumber, slotOcuppancy2.SlotNumber);
		}
	}
}