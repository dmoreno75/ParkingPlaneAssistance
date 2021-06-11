using System;
using System.Collections.Generic;

namespace FrozenLand.PlaneParkingAssistant.Core
{
	public class PlaneModelTypesMappingRepository
	{
		static IDictionary<string, PlaneType> _repository;

		public PlaneModelTypesMappingRepository(IDictionary<string, PlaneType> mapping)
		{
			_repository = mapping;
		}
		public PlaneType GetPlaneType(string model)
		{
			return _repository.TryGetValue(model, out var planeType) ? planeType : throw new NotRecognisedModelException(model);
		}
	}
}