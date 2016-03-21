using System;

namespace Starving.Models
{
	public class User
	{
		public string ObjectId { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string Username { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public File ProfilePicture { get; set; }

		public string __type;

		public string ClassName;

	}
}

