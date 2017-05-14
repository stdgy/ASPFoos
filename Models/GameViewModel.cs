using System;
using System.Collections.Generic;

namespace foosball_asp.Models 
{
    class ScoreViewModel
    {
        public string Username { get; set; }
        public string Position { get; set; }
        public DateTime Time { get; set; }
        public bool OwnGoal { get; set; }
    }

    class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public int Score { get; set; }
    }

    class TeamViewModel
    {
        public PlayerViewModel Goalie { get; set; }
        public PlayerViewModel Defender { get; set; }
        public PlayerViewModel Center { get; set; }
        public PlayerViewModel Striker { get; set; }
    }
    
    class GameViewModel 
    {
        public TeamViewModel RedTeam { get; set; }
        public TeamViewModel BlueTeam { get; set; }

        public List<ScoreViewModel> Scores { get; set; }
    }
}