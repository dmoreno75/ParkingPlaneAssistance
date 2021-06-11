using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ParkingPlaneAssistant.Consumer.Commands
{
	public record APIResponse(string Message, bool WasSuccessful);
	public class APIRequests
	{
		private readonly HttpClient _client = null;
		private readonly string _baseUrl = "";
		private readonly string path;

		public APIRequests(string baseUrl, string path)
		{
			_client = new HttpClient();
			_baseUrl = baseUrl;
			this.path = path;
		}

		public async Task<APIResponse> Run(IDictionary<string, string> queryString)
		{
			var query = string.Join("&", queryString.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
			var requestUri = $"{_baseUrl}/{path}?{query}";
			try
			{
				var response = _client.GetAsync(requestUri).Result;

				if (response.IsSuccessStatusCode)
				{
					var c = response.Content.ReadAsStringAsync().Result;
					return JsonConvert.DeserializeObject<APIResponse>(c);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			throw new Exception("Error");
		}
	}
}
