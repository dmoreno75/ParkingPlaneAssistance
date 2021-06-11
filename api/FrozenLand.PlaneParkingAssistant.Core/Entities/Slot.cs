using System.Linq;
using System.Collections.Generic;
using FrozenLand.PlaneParkingAssistant.Core.Utils;
using System.ComponentModel;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public enum PlaneType
	{
		[Description("Prop")] 	Props = 1,
		[Description("Jet")]	Jets = 2,
		[Description("Jumbo")]	Jumbos = 3
	}

	public class Slot
	{
		public int Number { get; init; }
		public PlaneType Type { get; init; }
		public IEnumerable<SlotOcuppancy> Ocuppancy { get { return occupancy; } }

		private IList<SlotOcuppancy> occupancy = new List<SlotOcuppancy>();

		public Slot(int number, PlaneType type)
		{
			Number = number;
			Type = type;
		}

		public SlotOcuppancy Book(string model, DateTimeRange dateTimeRange)
		{
			foreach (var item in occupancy)
			{
				if (item.DateRange.Overlaps(dateTimeRange)) return null;
			}

			var bookingId = $"{Number}-{GuidGen.Get()}";
			var slotOcuppancy = new SlotOcuppancy(this.Number, bookingId, model, dateTimeRange);
			occupancy.Add(slotOcuppancy);

			return slotOcuppancy;
		}

		public bool Release(string bookingId)
		{
			var slotToRemove = Ocuppancy.Where(x => x.BookingId == bookingId).SingleOrDefault();

			return slotToRemove != null ?
				occupancy.Remove(slotToRemove) :
				false;
		}

		public bool Overlaps(DateTimeRange dateTimeRange)
		{
			foreach (var item in this.Ocuppancy)
			{
				if (item.DateRange.Overlaps(dateTimeRange))
					return true;
			}

			return false;
		}
	}




}
