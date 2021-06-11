namespace FrozenLand.PlaneParkingAssistant.Core.Interfaces
{
	public interface IParkingPlaneAssistant
	{
		Slot Recommend(string model, DateTimeRange dateTimeRange);
		SlotOcuppancy Book(string model, DateTimeRange dateTimeRange);
		bool Release(string bookingId);
		Slot Status (int slotNumber);
	}
}
