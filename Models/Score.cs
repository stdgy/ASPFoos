using System;

namespace foosball_asp.Models 
{
    public class Score {
        public int Id { get; set; }
        public bool OwnGoal { get; set; }
        public DateTime TimeScored { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}