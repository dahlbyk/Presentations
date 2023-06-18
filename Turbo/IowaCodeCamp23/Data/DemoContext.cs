using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IowaCodeCamp23.Models;

namespace IowaCodeCamp23.Data
{
    public class DemoContext : DbContext
    {
        public DemoContext (DbContextOptions<DemoContext> options)
            : base(options)
        {
        }

        public DbSet<IowaCodeCamp23.Models.Todo> Todo { get; set; } = default!;
    }
}
