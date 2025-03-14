using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace PA2.Models
{// Party events with detaikls like date location and invitations
    public class Party
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string Location { get; set; }

        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
