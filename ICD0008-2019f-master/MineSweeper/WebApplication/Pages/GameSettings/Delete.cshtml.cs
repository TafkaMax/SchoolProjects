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
    public class DeleteModel : PageModel
    {
        private readonly DAL.GameDbContext _context;

        public DeleteModel(DAL.GameDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public GameSetting GameSetting { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameSetting = await _context.GameSettings.FirstOrDefaultAsync(m => m.GameSettingId == id);

            if (GameSetting == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GameSetting = await _context.GameSettings.FindAsync(id);

            if (GameSetting != null)
            {
                _context.GameSettings.Remove(GameSetting);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
