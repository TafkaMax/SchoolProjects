using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApplication.Pages_GameSettings
{
    public class IndexModel : PageModel
    {
        private readonly DAL.GameDbContext _context;

        public IndexModel(DAL.GameDbContext context)
        {
            _context = context;
        }

        public IList<GameSetting> GameSetting { get;set; }

        public async Task OnGetAsync()
        {
            GameSetting = await _context.GameSettings.ToListAsync();
        }
    }
}
