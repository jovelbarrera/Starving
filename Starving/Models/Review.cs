using System;
using Newtonsoft.Json;

namespace Starving.Models
{
    public class Review
    {
        public string ObjectId { get; set; }

        public User User { get; set; }

        public string Comment { get; set; }

        public string PlaceId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

