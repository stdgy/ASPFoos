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

        private float GetAverageScore(User user)
        {
            var totalGames = user.Players
                .Select(p => p.Team)
                .Select(t => t.Game)
                .Distinct()
                .Where(g => g.EndDate != null)
                .Count();

            var totalScore = user.Players
                .Where(p => p.Team.Game.EndDate != null)
                .SelectMany(p => p.Scores)
                .Where(s => s.OwnGoal == false)
                .Count();

            if (totalGames > 0.9f)
            {
                return totalScore / (float)totalGames;
            }

            return 0.0f;
        }

        private int GetTotalWins(User user)
        {
            return user.Players
                .Select(p => p.Team)
                .Distinct() // Get distinct teams user has been on
                .Where(t => t.Game.EndDate != null) // Where the game is over
                .Where(t => t.Players // Count of my teams goals
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == false)
                    .Count() +
                    t.Game.Teams // Plus other teams own goals
                    .Where(ot => ot.Id != t.Id)
                    .SelectMany(ot => ot.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == true)
                    .Count() > // More than other teams combined goals
                    t.Game.Teams // Count of other teams goals
                    .Where(ot => ot.Id != t.Id)
                    .SelectMany(ot => ot.Players)
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == false)
                    .Count() +
                    t.Players // Plus our own goals
                    .SelectMany(p => p.Scores)
                    .Where(s => s.OwnGoal == true)
                    .Count())
                .Count();
        }

        public UsersController(FoosContext context)
        {
            _context = context;  
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = _context.Users
                .Include(u => u.Players)
                .ThenInclude(p => p.Team)
                .ThenInclude(t => t.Game)
                .Include(u => u.Players)
                .ThenInclude(p => p.Scores)
                .Select(u => new IndexViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    DisplayName = u.DisplayName,
                    Birthdate = u.Birthdate,
                    AverageScore = GetAverageScore(u),
                    TotalWins = GetTotalWins(u)
                });
            return View(await users.ToListAsync());
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
                .Include(u => u.Players)
                .ThenInclude(p => p.Team)
                .ThenInclude(t => t.Game)
                .Include(u => u.Players)
                .ThenInclude(p => p.Scores)
                .SingleOrDefaultAsync(m => m.Id == id);

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
                AverageScore = GetAverageScore(user),
                TotalWins = GetTotalWins(user)
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
