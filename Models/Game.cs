using System;
using System.Collections.Generic;

namespace foosball_asp.Models 
{
    public class Game {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Team> Teams { get; set; }
    }
}