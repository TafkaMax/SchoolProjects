using System;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameEngine;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages.Game
{
    public class Game : PageModel
    {
        [BindProperty] public string GameName { get; set; }

        [BindProperty] public int GameId { get; set; }


        public GameController GameEngine;

        private readonly GameDbContext _context;

        public Game(GameDbContext context)
        {
            _context = context;
        }

        public void OnGet(int? gameId, int? x, int? y, String? operation, int? gameSettingsId)
        {
            if (gameId == null)
            {
                if (gameSettingsId == null)
                {
                    GameEngine = new GameController(_context, new GameSetting());
                }
                else
                {
                    GameEngine = new GameController(_context, _context.Find<GameSetting>(gameSettingsId));
                }
            }
            else if (_context.Find<Domain.Game>(gameId) != null)
            {
                GameEngine = new GameController(_context, (int) gameId);
            }


            if (x != null && y != null && ((x >= 0 && x <= GameEngine.CurrentGame.Width) && (y >= 0 && y <= GameEngine.CurrentGame.Height)) )
            {
                if ("open".Equals(operation) && !GameEngine.CurrentGame.GameLost)
                {
                    GameEngine.OpenPosition((int) y, (int) x);
                }
                else if ("flag".Equals(operation) && !GameEngine.CurrentGame.GameLost)
                {
                    GameEngine.ChangeFlag((int) y, (int) x);
                }
            }
        }

        public RedirectToPageResult OnPost()
        {
            GameEngine = new GameController(_context, GameId) {CurrentGame = {GameName = GameName}};
            _context.SaveChanges();
            return RedirectToPage("../SaveGames/Index");
        }
    }
}