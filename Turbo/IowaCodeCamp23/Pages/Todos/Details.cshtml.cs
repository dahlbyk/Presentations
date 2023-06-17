using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using IowaCodeCamp23.Data;
using IowaCodeCamp23.Models;

namespace IowaCodeCamp23.Pages.Todos
{
    public class DetailsModel : PageModel
    {
        private readonly IowaCodeCamp23.Data.DemoContext _context;

        public DetailsModel(IowaCodeCamp23.Data.DemoContext context)
        {
            _context = context;
        }

      public Todo Todo { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Todo == null)
            {
                return NotFound();
            }

            var todo = await _context.Todo.FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            else 
            {
                Todo = todo;
            }
            return Page();
        }
    }
}
