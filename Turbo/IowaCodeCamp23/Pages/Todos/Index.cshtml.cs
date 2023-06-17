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
    public class IndexModel : PageModel
    {
        private readonly IowaCodeCamp23.Data.DemoContext _context;

        public IndexModel(IowaCodeCamp23.Data.DemoContext context)
        {
            _context = context;
        }

        public IList<Todo> Todo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Todo != null)
            {
                Todo = await _context.Todo.ToListAsync();
            }
        }
    }
}
