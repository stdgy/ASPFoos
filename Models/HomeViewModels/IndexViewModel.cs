using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using foosball_asp.Models;

namespace foosball_asp.Models.HomeViewModels
{
    public class LatestGameViewModel
    {
        public int GameId { get; set; }
        public DateTime? EndDate { get; set; }
        public TeamType Winner { get; set; }
    }

    public class HighestScoreViewModel
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public float AverageScore { get; set; }
    }

    public class ShameViewModel // Lowest average scores
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public float AverageScore { get; set; }
    }

    public class IndexViewModel
    {
        public List<LatestGameViewModel> LatestGames { get; set; }
        public List<HighestScoreViewModel> HighestScores { get; set; }
        public List<ShameViewModel> Shames { get; set; }
    }
}
