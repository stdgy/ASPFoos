using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foosball_asp.Models;
using foosball_asp.Models.UserViewModels;

namespace foosball_asp.Controllers
{
    public class UsersController : Controller
    {
        private readonly FoosContext _context;

        private async Task<float> GetAverageScore(User user)
        {
            var totalGames = _context.Players
                .Where(p => p.UserId == user.Id)
                .Select(p => p.Team)
                .Select(t => t.Game)
                .Distinct()
                .Where(g => g.EndDate != null)
                .CountAsync();

            var totalScore = _context.Players
                .Where(p => p.UserId == user.Id)
                .Where(p => p.Team.Game.EndDate != null)
                .SelectMany(p => p.Scores)
                .Where(s => s.OwnGoal == false)
                .CountAsync();

            await Task.WhenAll(new Task[] { totalGames, totalScore });

            if (totalGames.Result > 0.9f)
            {
                return totalScore.Result / (float)totalGames.Result;
            }

            return 0.0f;
        }

        private async Task<int> GetTotalWins(User user)
        {
            return await _context.Players
                .Include(p => p.Scores)
                .Include(p => p.Team)
                .ThenInclude(t => t.Game)
                .ThenInclude(g => g.Teams)
                .Select(p => new { Team = p.Team, Game = p.Team.Game })
                .Distinct()
                .Where(g => (g.Game
                    .Teams
                    .Where(t => t.Id == g.Team.Id)
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == false)
                    .Count()
                    +
                    g.Game
                    .Teams
                    .Where(t => t.Id != g.Team.Id)
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == true)
                    .Count())
                    >
                    (g.Game
                    .Teams
                    .Where(t => t.Id != g.Team.Id)
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == false)
                    .Count()
                    +
                    g.Game
                    .Teams
                    .Where(t => t.Id == g.Team.Id)
                    .SelectMany(t => t.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == true)
                    .Count()))
                .CountAsync();
        }

        private async Task<List<GameViewModel>> GetLatestGames(User user)
        {
            return await _context.Games
                .Where(g => g.EndDate != null)
                .Where(g => g.Teams
                    .SelectMany(t => t.Players)
                    .Where(p => p.UserId == user.Id)
                    .Count() > 1)
                .OrderBy(g => g.EndDate)
                .Take(10)
                .Select(g => new GameViewModel
                {
                    GameId = g.Id,
                    EndDate = g.EndDate,
                    Score = g.Teams
                        .SelectMany(t => t.Players)
                        .Where(p => p.UserId == user.Id)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == false)
                        .Count(),
                    Won = (g.Teams
                        .Where(t => t.Players.Where(p => p.UserId == user.Id).Count() > 0)
                        .SelectMany(t => t.Players)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == false)
                        .Count() +
                        g.Teams
                        .Where(t => t.Players.Where(p => p.UserId == user.Id).Count() == 0)
                        .SelectMany(t => t.Players)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == true)
                        .Count()) >
                        (g.Teams
                        .Where(t => t.Players.Where(p => p.UserId == user.Id).Count() == 0)
                        .SelectMany(t => t.Players)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == false)
                        .Count() +
                        g.Teams
                        .Where(t => t.Players.Where(p => p.UserId == user.Id).Count() > 0)
                        .SelectMany(t => t.Players)
                        .SelectMany(p => p.Scores)
                        .Where(s => s.OwnGoal == true)
                        .Count())
                }).ToListAsync();
        }

        public UsersController(FoosContext context)
        {
            _context = context;  
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderBy(u => u.DisplayName)
                .Take(50)
                .Select(u => new IndexViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Birthdate = u.Birthdate,
                    AverageScore = 0
                })
                .ToListAsync();
            
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Birthdate")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);

            var averageScore = await GetAverageScore(user);
            var totalWins = await GetTotalWins(user);

            if (user == null)
            {
                return NotFound();
            }
            return View(new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Birthdate = user.Birthdate,
                AverageScore = await GetAverageScore(user),
                TotalWins = await GetTotalWins(user),
                LatestGames = await GetLatestGames(user)
            });
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,DisplayName,Birthdate")] UserEditViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var u = _context.Users.SingleOrDefault(usermodel => usermodel.Id == id);

                if (u == null)
                {
                    return NotFound();
                }

                try
                {
                    u.UserName = user.UserName;
                    u.DisplayName = user.DisplayName;
                    u.Birthdate = user.Birthdate;

                    _context.Update(u);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .SingleOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
