using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using foosball_asp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace foosball_asp.Controllers
{
    public class ScoreController : Controller
    {
        private readonly FoosContext _context;

        public ScoreController(FoosContext context)
        {
            _context = context;
        }

        // POST: Score/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(int id, int? owngoal)
        {
            // Get player with that ID and add a score
            var player = await _context.Players
                .Include(p => p.Scores)
                .Include(p => p.Team)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (player == null)
            {
                return NotFound();
            }

            var score = new Score
            {
                OwnGoal = owngoal == null ? false : true,
                PlayerId = id,
                TimeScored = DateTime.Now
            };

            await _context.AddAsync(score);
            await _context.SaveChangesAsync();

            // Redirect back to edit screen 
            return RedirectToAction("Edit", "Games", new { id = player.Team.GameId });
        }

        // POST: Score/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(int id, int gameId)
        {
            var score = _context.Scores
                .SingleOrDefault(s => s.Id == id);

            if (score == null)
            {
                return NotFound();
            }

            _context.Remove(score);

            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Games", new { id = gameId });
        }
    }
}