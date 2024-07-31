using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using That23.Models;

    public class TodoDbContext : DbContext
    {
        public TodoDbContext (DbContextOptions<TodoDbContext> options)
            : base(options)
        {
        }

        public DbSet<That23.Models.Todo> Todo { get; set; } = default!;
    }
