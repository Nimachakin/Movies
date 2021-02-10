using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace MovieCatalog.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Movie> Movies { get; set; }

        public User()
        { }
    }
}