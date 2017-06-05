using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using foosball_asp.Models.HomeViewModels;
using foosball_asp.Models;

namespace foosball_asp.Controllers
{
    public class HomeController : Controller
    {
        private FoosContext _context;

        private async Task<List<LatestGameViewModel>> GetLatestGames()
        {
            return await _context.Games
                .Where(g => g.EndDate != null)
                .OrderBy(g => g.EndDate)
                .Take(10)
                .Select(g => new LatestGameViewModel
                {
                    GameId = g.Id,
                    EndDate = g.EndDate,
                    Winner = TeamType.Red
                })
                .ToListAsync();
        }

        public HomeController(FoosContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
