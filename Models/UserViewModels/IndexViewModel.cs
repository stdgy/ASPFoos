﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace foosball_asp.Models.UserViewModels
{
    public class IndexViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        [Required]
        [Display(Name = "Birthday")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Average Score")]
        public float AverageScore { get; set; }

        [Display(Name = "Total Wins")]
        public int TotalWins { get; set; }
    }
}
