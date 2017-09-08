using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class EventsViewModels
    {
        public EventsViewModels()
        {
            SportsList = new HashSet<Sports>();
        }
        public int EventId { get; set; }
        [Display(Name = "Event Name")]
        public string EventName { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Event Start Date")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Event End Date")]
        public DateTime EndDate { get; set; }


        public int SportId { get; set; }
        [Display(Name = "Sport Name")]
        public string SportName { get; set; }

        public long UserId { get; set; }
        public IEnumerable<Venues> Venue { get; set; }
        public int VenueId { get; set; }
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; }

        public IEnumerable<Sports> SportsList { get; set; }

        public bool IsActive { get; set; }
    }
}