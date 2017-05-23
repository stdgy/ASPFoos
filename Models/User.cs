using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models 
{
    public class User : IdentityUser {
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}