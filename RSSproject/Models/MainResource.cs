using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSproject.Models
{
    public class MainResource
    {
        public int Id { get; set; }

        public string URL { get; set; }

        public int CollectionId { get; set; }
    }
}