using System;
using System.Linq;
using System.Collections.Generic;
using FrozenLand.PlaneParkingAssistant.Core.Interfaces;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public class ParkingPlaneAssistant : IParkingPlaneAssistant
	{
		private readonly IList<SlotsConfiguration> slotsConfiguration;
		private readonly PlaneModelTypesMappingRepository mapping;

		private IDictionary<PlaneType, IList<Slot>> slots = new Dictionary<PlaneType, IList<Slot>>()
		{
			{PlaneType.Props, new List<Slot>() },
			{PlaneType.Jets, new List<Slot>() },
			{PlaneType.Jumbos, new List<Slot>() }
		};

		public ParkingPlaneAssistant(IList<SlotsConfiguration> slotsConfiguration, PlaneModelTypesMappingRepository mapping)
		{
			this.slotsConfiguration = slotsConfiguration;
			this.mapping = mapping;

			CreateSlots();
		}

		public Slot Recommend(string model, DateTimeRange dateTimeRange)
		{
			var planeType = mapping.GetPlaneType(model);
			return CheckAvailability(planeType, dateTimeRange) ?? throw new NotSlotsAvailableException();
		}

		public SlotOcuppancy Book(string model, DateTimeRange dateTimeRange)
		{
			var slot = Recommend(model, dateTimeRange);

			return (slot != null) ?
				slot.Book(model, dateTimeRange) :
				null;
		}

		public bool Release(string bookingId)
		{
			var slotNumber = bookingId.Split('-')[0];

			var slot = Status(int.Parse(slotNumber));

			return slot.Release(bookingId) ?
				true :
				throw new NotBookingIdFoundException(bookingId);
		}

		public Slot Status(int slotNumber)
		{
			var slot = slots[PlaneType.Props].Where(x => x.Number == slotNumber).FirstOrDefault();

			if (slot == null)
				slot = slots[PlaneType.Jets].Where(x => x.Number == slotNumber).FirstOrDefault();

			if (slot == null)
				slot = slots[PlaneType.Jumbos].Where(x => x.Number == slotNumber).FirstOrDefault();

			return slot ?? throw new NotSlotFoundException(slotNumber);
		}

		private void CreateSlots()
		{
			// Create plane parking area

			int slotIndex = 1;

			var propsDetails = this.slotsConfiguration.SingleOrDefault(x => x.Type == PlaneType.Props);
			if (propsDetails != null)
			{
				for (int i = 1; i <= propsDetails.MaxPlaces; i++)
				{
					var list = slots[PlaneType.Props];
					list.Add(new Slot(slotIndex++, PlaneType.Props));
				}
			}

			var jetsDetails = this.slotsConfiguration.SingleOrDefault(x => x.Type == PlaneType.Jets);
			if (jetsDetails != null)
			{
				for (int i = 1; i <= jetsDetails.MaxPlaces; i++)
				{
					var list = slots[PlaneType.Jets];
					list.Add(new Slot(slotIndex++, PlaneType.Jets));
				}
			}

			var jumboDetails = this.slotsConfiguration.SingleOrDefault(x => x.Type == PlaneType.Jumbos);
			if (jumboDetails != null)
			{
				for (int i = 1; i <= jumboDetails.MaxPlaces; i++)
				{
					var list = slots[PlaneType.Jumbos];
					list.Add(new Slot(slotIndex++, PlaneType.Jumbos));
				}
			}
		}

		private Slot CheckAvailability(PlaneType type, DateTimeRange dateTimeRange)
		{
			Slot result = CheckAvailabilityForType(type, dateTimeRange);

			switch (type)
			{
				case PlaneType.Props:
					return result ?? CheckAvailability(PlaneType.Jets, dateTimeRange);
				case PlaneType.Jets:
					return result ?? CheckAvailability(PlaneType.Jumbos, dateTimeRange);
			}

			return result;
		}

		private Slot CheckAvailabilityForType(PlaneType type, DateTimeRange dateTimeRange)
		{
			if (!slots.ContainsKey(type)) return null;

			var busyPlaces = slots[type];

			return busyPlaces.Count > 0 ?
				busyPlaces.FirstOrDefault(s => !s.Overlaps(dateTimeRange)) :
				null;
		}
	}
}
