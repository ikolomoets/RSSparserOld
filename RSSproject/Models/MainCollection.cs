using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RSSproject.Models
{
    public class MainCollection
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<MainResource> MainResources { get; set; }

        public MainCollection()
        {
            MainResources = new List<MainResource>();
        }
    }
}