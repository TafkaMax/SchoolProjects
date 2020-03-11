using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApplication.Pages_GameSettings
{
    public class EditModel : PageModel
    {
        private readonly DAL.GameDbContext _context;

        public EditModel(DAL.GameDbContext context)
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GameSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameSettingExists(GameSetting.GameSettingId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GameSettingExists(int id)
        {
            return _context.GameSettings.Any(e => e.GameSettingId == id);
        }
    }
}
