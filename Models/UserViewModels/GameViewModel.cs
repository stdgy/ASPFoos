using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace foosball_asp.Models.UserViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public int Score { get; set; }

        public Boolean Won { get; set; }
    }
}
