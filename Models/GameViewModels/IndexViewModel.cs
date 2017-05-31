using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace foosball_asp.Models.GameViewModels
{
    public enum GameStatus
    {
        Ended,
        Ongoing
    }

    public class IndexViewModel
    {
        public int GameId { get; set; }

        [Display(Name = "Red Score")]
        public int RedScore { get; set; }

        [Display(Name = "Blue Score")]
        public int BlueScore { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        public GameStatus Status { get; set; }
    }
}
