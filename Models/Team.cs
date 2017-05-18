using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace foosball_asp.Models 
{
    public enum TeamType {
        [DataType(DataType.Text)]
        [Display(Name = "Blue")]
        Blue,
        [DataType(DataType.Text)]
        [Display(Name = "Red")]
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