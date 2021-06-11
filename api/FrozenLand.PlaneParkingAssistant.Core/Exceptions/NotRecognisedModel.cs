using System;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public class AssistantException : Exception
	{
		public AssistantException(string message): base(message)
		{

		}
	}

	public class NotRecognisedModelException : AssistantException
	{
		public NotRecognisedModelException(string model): base($"The model {model} was not recognised")
		{

		}
	}

	public class NotSlotsAvailableException : AssistantException
	{
		public NotSlotsAvailableException() : base($"There are no slots available at this moment")
		{

		}
	}

	public class NotSlotFoundException : AssistantException
	{
		public NotSlotFoundException(int slotNumber) : base($"The slot number {slotNumber} was not found")
		{

		}
	}

	public class NotBookingIdFoundException : AssistantException
	{
		public NotBookingIdFoundException(string bookingId) : base($"The booking identifier {bookingId} was not found")
		{

		}
	}

	public class IncorrectDateTimeRangeException : AssistantException
	{
		public IncorrectDateTimeRangeException() : base($"The dates given are incorrect")
		{

		}
	}
}
