using FrozenLand.PlaneParkingAssistant.Core;
using FrozenLand.PlaneParkingAssistant.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrozenLand.PlaneParking.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ParkingPlaneAssistantController : ControllerBase
	{
		private readonly ILogger<ParkingPlaneAssistantController> _logger;
		private readonly IParkingPlaneAssistant _parkingPlaceAssistant;

		public ParkingPlaneAssistantController(
			ILogger<ParkingPlaneAssistantController> logger,
			IParkingPlaneAssistant parkingPlaceAssistant
			)
		{
			_logger = logger;
			_parkingPlaceAssistant = parkingPlaceAssistant;
		}

		[HttpGet]
		[Route("book")]
		public ResponseViewModel Book([FromQuery] string model, [FromQuery] DateTime start, [FromQuery] DateTime end)
		{
			try
			{
				var slotOcuppancy = _parkingPlaceAssistant.Book(model, new DateTimeRange(start, end));
				return new ResponseViewModel($"Slot {slotOcuppancy.SlotNumber} booked between {slotOcuppancy.DateRange.Start} and {slotOcuppancy.DateRange.End} with booking identifier {slotOcuppancy.BookingId}");
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Error caught: {ex.Message}");
				return new ResponseViewModel(ex);
			}
			
		}

		[HttpGet]
		[Route("recommend")]
		public RecommendViewModel Recommend([FromQuery] string model)
		{
			try
			{
				var slot = _parkingPlaceAssistant.Recommend(model, new DateTimeRange(DateTime.Now, DateTime.Now.AddDays(1)));
				return new RecommendViewModel(slot, model);
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Error caught: {ex.Message}");
				return new RecommendViewModel(ex);
			}
		}

		[HttpGet]
		[Route("release")]
		public ResponseViewModel Release([FromQuery] string bookingId)
		{
			try
			{
				var result = _parkingPlaceAssistant.Release(bookingId);
				return new ResponseViewModel($"Slot with identifier {bookingId} has been released");
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Error caught in Release Action: {ex.Message}");
				return new ResponseViewModel(ex);
			}
		}

		[HttpGet]
		[Route("status")]
		public ResponseViewModel Status([FromQuery] int slotNumber)
		{
			try
			{
				var slot = _parkingPlaceAssistant.Status(slotNumber);
				return new ResponseViewModel($"Slot {slotNumber} has {slot.Ocuppancy.Count()} bookings:\n\t{string.Join("\n\t", slot.Ocuppancy.Select(x=> $"{x.DateRange.Start}-{x.DateRange.End} (identifier {x.BookingId})"))}");
			}
			catch (Exception ex)
			{
				_logger.LogInformation($"Error caught: {ex.Message}");
				return new ResponseViewModel(ex);
			}
		}

		[HttpGet]
		[Route("prebook")]
		public ResponseViewModel Prebook([FromQuery] string model, [FromQuery] DateTime start, [FromQuery] DateTime end)
		{
			return null;
		}
	}
}
