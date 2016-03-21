using System;
using Newtonsoft.Json;

namespace Starving.Models
{
	public class Rate
	{
		public string ObjectId{ get; set; }

		public User User { get; set; }

		public Place Place { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}

