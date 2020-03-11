using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace BookApp.Pages.Books
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public SelectList LanguageSelectList { get; set; }
        public SelectList PublisherSelectList { get; set; }

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            LanguageSelectList = new SelectList(_context.Languages, "LanguageId", "LanguageName");
            PublisherSelectList = new SelectList(_context.Publishers, "PublisherId", "PublisherName");
            return Page();
        }

        [BindProperty] public Book Book { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Books.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}