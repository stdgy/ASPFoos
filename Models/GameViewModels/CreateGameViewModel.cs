using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace foosball_asp.Models.GameViewModels
{
    public class CreateTeamViewModel {

            [Display(Name="Goalie")]
            public string GoalieId { get; set; }

            [Display(Name="Defender")]
            public string DefenderId { get; set; }

            [Display(Name="Center")]
            public string CenterId { get; set; }

            [Display(Name="Striker")]
            public string StrikerId { get; set; }
    }
    public class CreateGameViewModel 
    {
        public CreateTeamViewModel RedTeam { get; set; }
        public CreateTeamViewModel BlueTeam { get; set; }

        public List<SelectListItem> Users { get; set; }
    }
}