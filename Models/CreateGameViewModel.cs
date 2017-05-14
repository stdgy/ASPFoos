using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace foosball_asp.Models
{
    public class CreateTeamViewModel {

            [Display(Name="Goalie")]
            public int GoalieId { get; set; }

            [Display(Name="Defender")]
            public int DefenderId { get; set; }

            [Display(Name="Center")]
            public int CenterId { get; set; }

            [Display(Name="Striker")]
            public int StrikerId { get; set; }
    }
    public class CreateGameViewModel 
    {
        public CreateTeamViewModel RedTeam { get; set; }
        public CreateTeamViewModel BlueTeam { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}