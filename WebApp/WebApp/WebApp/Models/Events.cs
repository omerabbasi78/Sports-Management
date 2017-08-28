﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models
{
    public class Events
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [ForeignKey("SportId")]
        public Sports Sport { get; set; }
        
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public Users User { get; set; }
        [ForeignKey("VenueId")]
        public Venues Venue { get; set; }
        public int VenueId { get; set; }
        
    }
}