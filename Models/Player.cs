using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models 
{
    public enum PlayerType {
        Goalie,
        Defender,
        Center,
        Striker
    }

    public class Player {
        public int Id { get; set; }
        public PlayerType Type { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public List<Score> Scores { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}