using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApp.Models
{
    public class UserChallenges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserChallengeId { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name ="Challenge Accepted")]
        public bool IsAccepted { get; set; }


        [ForeignKey("Users")]
        public int UserId { get; set; }
        public Users User { get; set; }
        [ForeignKey("Events")]
        public int EventId { get; set; }
        public Events Event { get; set; }
    }
}