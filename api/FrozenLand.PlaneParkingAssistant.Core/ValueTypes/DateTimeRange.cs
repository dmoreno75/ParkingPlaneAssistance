using System;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public record DateTimeRange(DateTime Start, DateTime End)
	{
		public bool Overlaps(DateTimeRange dateTimeRange)
		{
			return this.Start < dateTimeRange.End &&
				this.End > dateTimeRange.Start;
		}
	}
}
