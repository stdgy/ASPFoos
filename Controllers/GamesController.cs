using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using foosball_asp.Models;

namespace foosball_asp.Controllers
{
    public class GamesController : Controller
    {
        private readonly FoosContext _context;

        public GamesController(FoosContext context)
        {
            _context = context;    
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Games.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            var gvm = new CreateGameViewModel();
            gvm.Users = 
                ( from user in _context.Users 
                select new SelectListItem{ Value = user.Id.ToString(), Text = user.Username } )
                .ToList();

            return View(gvm);
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel game)
        {
            if (ModelState.IsValid)
            {
                // Use the passed in data to create new game objects
                
                // Create game
                var newGame = new Game {
                    StartDate = DateTime.Now,
                    Teams = new List<Team> {
                        new Team {
                            Type = TeamType.Red,
                            Players = new List<Player> {
                                new Player {
                                    Type = PlayerType.Goalie,
                                    UserId = game.RedTeam.GoalieId
                                },
                                new Player {
                                    Type = PlayerType.Defender,
                                    UserId = game.RedTeam.DefenderId
                                },
                                new Player {
                                    Type = PlayerType.Center,
                                    UserId = game.RedTeam.CenterId
                                },
                                new Player {
                                    Type = PlayerType.Striker,
                                    UserId = game.RedTeam.StrikerId
                                }
                            }
                        },
                        new Team {
                            Type = TeamType.Blue,
                            Players = new List<Player> {
                                new Player {
                                    Type = PlayerType.Goalie,
                                    UserId = game.BlueTeam.GoalieId
                                },
                                new Player {
                                    Type = PlayerType.Defender,
                                    UserId = game.BlueTeam.DefenderId
                                },
                                new Player {
                                    Type = PlayerType.Center,
                                    UserId = game.BlueTeam.CenterId
                                },
                                new Player {
                                    Type = PlayerType.Striker,
                                    UserId = game.BlueTeam.StrikerId
                                }
                            }
                        }
                    }
                };
                
                _context.Add(newGame);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Make sure we use includes to gather all related data that we'll need
            // to construct the view model below.
            var game = await _context.Games
                .Where(g => g.Id == id)
                .Include(g => g.Teams)
                    .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.Scores)
                .Include(g => g.Teams)
                    .ThenInclude(t => t.Players)
                        .ThenInclude(p => p.User)
                .SingleOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            // Build up a game view model
            var gvm = new GameViewModel {
                RedTeam = new TeamViewModel {
                    Goalie = new PlayerViewModel {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Defender = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Center = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Striker = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Red).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().Scores.Where(s => s.OwnGoal == false).Count()
                    }
                },
                BlueTeam = new TeamViewModel
                {
                    Goalie = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Goalie).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Defender = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Defender).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Center = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Center).First().Scores.Where(s => s.OwnGoal == false).Count()
                    },
                    Striker = new PlayerViewModel
                    {
                        PlayerId = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().Id,
                        Username = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().User.Username,
                        Score = game.Teams.Where(t => t.Type == TeamType.Blue).First()
                            .Players.Where(p => p.Type == PlayerType.Striker).First().Scores.Where(s => s.OwnGoal == false).Count()
                    }
                },
                Scores = game.Teams.SelectMany(t => t.Players).SelectMany(p => p.Scores).Select(s => new ScoreViewModel
                {
                    Username = s.Player.User.Username,
                    Position = "Placeholder",
                    Time = s.TimeScored,
                    OwnGoal = s.OwnGoal
                }).ToList()
            };

            return View(gvm);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartDate,EndDate")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
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
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .SingleOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.SingleOrDefaultAsync(m => m.Id == id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
