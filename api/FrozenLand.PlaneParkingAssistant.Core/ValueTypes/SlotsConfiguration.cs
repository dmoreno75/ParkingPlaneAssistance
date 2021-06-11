namespace FrozenLand.PlaneParkingAssistant.Core
{
	public record SlotsConfiguration(int MaxPlaces, PlaneType Type)
	{
		public static SlotsConfiguration Builder(int maxPlaces, PlaneType type)
		{
			return new SlotsConfiguration(maxPlaces, type);
		}
	}
}
