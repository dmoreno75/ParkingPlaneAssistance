using FrozenLand.PlaneParkingAssistant.Core;
using System;

namespace FrozenLand.PlaneParking.API.Controllers
{
	public class ViewModel<T> where T : class
	{
		public bool WasSuccessful { get; protected set; }
		public string Message { get; protected set; }

		public ViewModel()
		{
			WasSuccessful = true;
		}

		public ViewModel(Exception ex)
		{
			Message = ex.Message;
			WasSuccessful = false;
		}
	}

	public class RecommendViewModel : ViewModel<Slot>
	{
		public RecommendViewModel(Slot slot, string model) : base()
		{
			Message = $"Slot {slot.Number} is available for model {model}";
		}
		public RecommendViewModel(Exception ex) : base(ex) { }
	}

	public class ResponseViewModel : ViewModel<string>
	{
		public ResponseViewModel(string message) : base() 
		{
			Message = message;
		}
		public ResponseViewModel(Exception ex) : base(ex) { }
	}
}