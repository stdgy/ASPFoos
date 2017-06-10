using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using foosball_asp.Models;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models.HomeViewModels
{
    public class LatestGameViewModel
    {
        public int GameId { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Winner")]
        public TeamType Winner { get; set; }
    }

    public class HighestScoreViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Average Score")]
        public float AverageScore { get; set; }
    }

    public class ShameViewModel // Lowest average scores
    {
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string DisplayName { get; set; }

        [Display(Name = "Average Score")]
        public float AverageScore { get; set; }
    }

    public class IndexViewModel
    {
        [Display(Name = "Latest Games")]
        public List<LatestGameViewModel> LatestGames { get; set; }

        [Display(Name = "Highest Scorers")]
        public List<HighestScoreViewModel> HighestScores { get; set; }

        [Display(Name = "Lowest Scorers")]
        public List<ShameViewModel> Shames { get; set; }
    }
}
