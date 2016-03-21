using System;
using System.Threading.Tasks;

namespace Starving.Dependencies
{
	public class FacebookEvent
	{
		public string UserId { get; set; }
		public string AccessToken { get; set; }
		public DateTime TokenExpiration { get; set; }
	}
}

