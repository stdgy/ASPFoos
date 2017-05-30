using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace foosball_asp.Models
{
    public class CompleteGameViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int GameId { get; set; }
        public TeamViewModel RedTeam { get; set; }
        public TeamViewModel BlueTeam { get; set; }

        public List<ScoreViewModel> Scores { get; set; }
    }
}
