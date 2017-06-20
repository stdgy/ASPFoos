using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models 
{
    public class User : IdentityUser {
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public List<Player> Players { get; set; }
    }
}