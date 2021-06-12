using FrozenLand.PlaneParkingAssistant.Core.Tests.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace FrozenLand.PlaneParkingAssistant.Core.Tests
{
	public class RecommendingTests
	{
		private PlaneModelTypesMappingRepository mappingRepository;

		[SetUp]
		public void Setup()
		{
			mappingRepository = new PlaneModelTypesMappingRepository(SettingsConfigHelper.PlaneModelTypesMapping());
		}

		[Test]
		public void WithSmallParking_RecommendsWillReturnAvailableSlotIfthereIsAny()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(1, PlaneType.Props),

			}, mappingRepository);

			var slot = parkingSpace.Recommend("E195", dateTimeRange);
			Assert.IsNotNull(slot);
			Assert.AreEqual(PlaneType.Props, slot.Type);
		}


		[Test]
		public void WithMediumParking_RecommendsWillReturnAvailableSlotIfthereIsAny()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(0, PlaneType.Props),
				SlotsConfiguration.Builder(1, PlaneType.Jets),

			}, mappingRepository);

			var slot = parkingSpace.Recommend("E195", dateTimeRange);

			Assert.IsNotNull(slot);
			Assert.AreEqual(PlaneType.Jets, slot.Type);
		}


		[Test]
		public void WhenRecommendsItReturnsNullIfNotAvailableSlots()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(1, PlaneType.Props),
			}, mappingRepository);

			var slot = parkingSpace.Recommend("A380", dateTimeRange);
			Assert.IsNull(slot);
		}

		[Test]
		public void WhenRecommendsIReturnsNotRecogniseModelIfModelIsUnknown()
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(1, PlaneType.Props),
			}, mappingRepository);

			Assert.Throws<NotRecognisedModelException>(new TestDelegate(() => parkingSpace.Recommend("XXX", dateTimeRange)));

		}

		[TestCase(1, 1, 0, "E195", "E195", PlaneType.Jets, false)]
		[TestCase(2, 1, 0, "E195", "E195", PlaneType.Props, false)]
		[TestCase(0, 2, 0, "E195", "E195", PlaneType.Jets, false)]
		[TestCase(2, 0, 0, "E195", "E195", PlaneType.Props, false)]
		[TestCase(0, 0, 2, "E195", "E195", PlaneType.Jumbos, false)]
		[TestCase(1, 2, 0, "A330", "E195", PlaneType.Props, false)]
		[TestCase(1, 0, 1, "E195", "E195", PlaneType.Jumbos, false)]
		[TestCase(0, 1, 1, "A380", "E195", PlaneType.Jets, false)]
		[TestCase(1, 0, 1, "A380", "E195", PlaneType.Props, false)]
		[TestCase(0, 0, 1, "A380", "E195", PlaneType.Jumbos, true)]
		public void WhenRecommendsAndNotPropsParkingAvailable_ItReturnsJetsAvailableSlot(
			int propsSpaces, int jetsSpaces, int jumbosSpaces,
			string modelBooked, string modelRecommended,
			PlaneType expectedType, bool recommendReturnsNull)
		{
			var dateTimeRange = new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1));

			var parkingSpace = new ParkingPlaneAssistant(new List<SlotsConfiguration>()
			{
				SlotsConfiguration.Builder(propsSpaces, PlaneType.Props),
				SlotsConfiguration.Builder(jetsSpaces, PlaneType.Jets),
				SlotsConfiguration.Builder(jumbosSpaces, PlaneType.Jumbos),
			}, mappingRepository);

			parkingSpace.Book(modelBooked, new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1)));
			var slot = parkingSpace.Recommend(modelRecommended, dateTimeRange);
			if (recommendReturnsNull)
			{
				Assert.IsNull(slot);
			}
			else
			{
				Assert.IsNotNull(slot);
				Assert.AreEqual(expectedType, slot.Type);
			}
		}
	}
}