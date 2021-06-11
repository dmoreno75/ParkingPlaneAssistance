using FrozenLand.PlaneParkingAssistant.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace FrozenLand.ParkingPlaneAssistant.Persistence
{
	public class AppContext: DbContext
	{
		public DbSet<Slot> Slots { get; set; }
	}
}
