using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApplication.Pages_SaveGames
{
    public class IndexModel : PageModel
    {
        private readonly DAL.GameDbContext _context;

        public IndexModel(DAL.GameDbContext context)
        {
            _context = context;
        }

        public IList<Game> Game { get;set; }
        public IList<Game> GamesNotLostOrWon { get; set; }

        public async Task OnGetAsync()
        {
            Game = await _context.Games.ToListAsync();
            GamesNotLostOrWon = new List<Game>();
            foreach (var game in Game)
            {
                if (game.GameLost || game.GameWon)
                {
                    _context.Games.Remove(game);
                    _context.SaveChanges();
                }
                else
                {
                    GamesNotLostOrWon.Add(game);
                }
            }

            Game = GamesNotLostOrWon;

        }
    }
}
