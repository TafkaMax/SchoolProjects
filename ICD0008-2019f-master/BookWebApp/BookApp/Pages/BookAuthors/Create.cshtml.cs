using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace BookApp.Pages.BookAuthors
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public SelectList SelectAuthorList { get; set; }
        public SelectList SelectBookList { get; set; }

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        SelectAuthorList = new SelectList(_context.Authors, "AuthorId", "FirstLastName");
        SelectBookList = new SelectList(_context.Books, "BookId", "Title");
            return Page();
        }

        [BindProperty]
        public BookAuthor BookAuthor { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.BookAuthors.Add(BookAuthor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
