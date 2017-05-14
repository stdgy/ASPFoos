using System.Collections.Generic;

namespace foosball_asp.Models 
{
    public enum TeamType {
        Blue,
        Red
    }

    public class Team {
        public int Id { get; set; }
        public TeamType Type { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public List<Player> Players { get; set; }
    }
}