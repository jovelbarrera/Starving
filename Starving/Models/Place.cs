using System;
using Newtonsoft.Json;

namespace Starving.Models
{
	public class Place
	{
		public Geometry Geometry { get; set; }
		public string Icon { get; set; }
		public string Name { get; set; }

		[JsonProperty ("opening_hours")]
		public OpeningHours OpeningHours { get; set; }

		public Photos[] Photos{ get; set; }

		[JsonProperty ("place_id")]
		public string PlaceId { get; set; }

		public string[] Types{ get; set; }
		public string Vicinity { get; set; }
	}

	public class Geometry
	{
		public Location Location { get; set; }
	}

	public class OpeningHours
	{
		[JsonProperty ("open_now")]
		public bool IsOpenNow{ get; set; }
	}

	public class Photos
	{
		public float Height{ get; set; }
		[JsonProperty ("photo_reference")]
		public string PhotoReference{ get; set; }
		public float Width{ get; set; }
	}


	public class Location
	{
		[JsonProperty ("lat")]
		public double Latitude { get; set; }

		[JsonProperty ("lng")]
		public double Longitude { get; set; }
	}
}

