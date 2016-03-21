using System;
using System.Collections.Generic;

namespace Starving.Models
{
	public class GoogleResponse
	{
		public string HtmlAttributions { get; set; }

		public string NextPageToken { get; set; }

		public List<Place> Results { get; set; }

		public Place Result { get; set; }

		public string Status { get; set; }
	}
}

