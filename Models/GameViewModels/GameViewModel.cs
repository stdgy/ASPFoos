using System;
using System.Collections.Generic;

namespace foosball_asp.Models.GameViewModels
{
    public class ScoreViewModel
    {
        public int ScoreId { get; set; }
        public string Username { get; set; }
        public PlayerType Position { get; set; }
        public DateTime Time { get; set; }
        public bool OwnGoal { get; set; }
        public TeamType Team { get; set; }
    }

    public class PlayerViewModel
    {
        public int PlayerId { get; set; }
        public string Username { get; set; }
        public int Score { get; set; }
        public PlayerType Position { get; set; }
    }

    public class TeamViewModel
    {
        public PlayerViewModel Goalie { get; set; }
        public PlayerViewModel Defender { get; set; }
        public PlayerViewModel Center { get; set; }
        public PlayerViewModel Striker { get; set; }

        public int Score { get; set; }
    }
    
    public class GameViewModel 
    {
        public int GameId { get; set; }
        public TeamViewModel RedTeam { get; set; }
        public TeamViewModel BlueTeam { get; set; }

        public List<ScoreViewModel> Scores { get; set; }
    }
}