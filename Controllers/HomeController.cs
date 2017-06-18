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

        private async Task<List<HighestScoreViewModel>> GetHighestScores()
        {
            return await _context.Users
                .Include(u => u.Players)
                .Select(u => new
                {
                    UserId = u.Id,
                    DisplayName = u.DisplayName,
                    TotalGames = u.Players
                        .Select(p => p.Team)
                        .Where(t => t.Game.EndDate != null)
                        .Select(t => t.GameId)
                        .Distinct()
                        .Count(),
                    TotalScore = u.Players
                        .Where(p => p.Team.Game.EndDate != null)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == false)
                        .Count()
                })
                .Select(u => new
                {
                    UserId = u.UserId,
                    DisplayName = u.DisplayName,
                    Average = u.TotalGames > 0 ? (u.TotalScore / (float)u.TotalGames) : 0.0f
                })
                .OrderByDescending(u => u.Average)
                .Take(10)
                .Select(u => new HighestScoreViewModel
                {
                    UserId = u.UserId,
                    DisplayName = u.DisplayName,
                    AverageScore = u.Average
                })
                .ToListAsync();
        }

        private async Task<List<ShameViewModel>> GetLowestScores()
        {
            return await _context.Users
                .Include(u => u.Players)
                .Select(u => new
                {
                    UserId = u.Id,
                    DisplayName = u.DisplayName,
                    TotalGames = u.Players
                        .Select(p => p.Team)
                        .Where(t => t.Game.EndDate != null)
                        .Select(t => t.GameId)
                        .Distinct()
                        .Count(),
                    TotalScore = u.Players
                        .Where(p => p.Team.Game.EndDate != null)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == false)
                        .Count()
                })
                .Select(u => new
                {
                    UserId = u.UserId,
                    DisplayName = u.DisplayName,
                    Average = u.TotalGames > 0 ? (u.TotalScore / (float)u.TotalGames) : 0.0f
                })
                .OrderBy(u => u.Average)
                .Take(10)
                .Select(u => new ShameViewModel
                {
                    UserId = u.UserId,
                    DisplayName = u.DisplayName,
                    AverageScore = u.Average
                })
                .ToListAsync();
        }

        public HomeController(FoosContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var LatestGames =  await GetLatestGames();
            var HighScores =  await GetHighestScores();
            var LowScores =  await GetLowestScores();
            
            return View(new IndexViewModel
            {
                LatestGames = LatestGames,
                HighestScores = HighScores,
                Shames = LowScores
            });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
