using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using That23.Models;

namespace That23.Pages_Todos
{
    public class IndexModel : PageModel
    {
        private readonly TodoDbContext _context;

        public IndexModel(TodoDbContext context)
        {
            _context = context;
        }

        public IList<Todo> Todos { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Todo != null)
            {
                Todos = await _context.Todo.ToListAsync();
            }
        }

        [BindProperty]
        public Todo Todo { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Todo == null || Todo == null)
            {
                return Page();
            }

            _context.Todo.Add(Todo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
