using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;

namespace MovieCatalog.Data
{
    public class MoviesContext : IdentityDbContext<User>
    {
        public DbSet<Movie> Movies { get; set; }
        
        public MoviesContext(DbContextOptions<MoviesContext> options)
            : base(options)
        { 
            Database.EnsureCreated(); 
        }
    }
}
