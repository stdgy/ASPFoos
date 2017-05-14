using System;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models 
{
    public class User {
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }
    }
}