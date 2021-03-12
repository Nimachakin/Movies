using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.Data;
using System.Collections.Generic;
using MovieCatalog.Models;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MovieCatalog.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly UserManager<User> _userManager;
        MoviesContext context; 

        public MovieController(MoviesContext db, UserManager<User> userManager)
        {
            context = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<MovieJson> movies = await 
                (from m in context.Movies 
                 select new MovieJson{ 
                    Id = m.Id, 
                    Name = m.Name, 
                    Year = m.ProductionYear, 
                    Director = m.Director, 
                    UserName = m.User.UserName })
                .ToListAsync();

            return Json(movies);
        }

        [HttpGet]
        public async Task<ActionResult> Get(int id)
        {
            var movie = await context.Movies
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
            
            if(movie != null)
            {
                return PartialView("_DetailsPartial", movie);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Добавление нового фильма";
            ViewData["BannerBtn"] = "Добавить постер";
            ViewData["Action"] = "Create";            
            return PartialView("_EditPartial");
        }

        [HttpPost]
        public async Task<ActionResult> Create(MovieEditModel model)
        {
            if(ModelState.IsValid)
            {
                var movie = new Movie() {
                    Name = model.Name, 
                    ProductionYear = model.ProductionYear ?? 0, 
                    Director = model.Director, 
                    Description = model.Description
                };

                movie.User = await _userManager.FindByNameAsync(User.Identity.Name);
                
                if(model.BannerImage != null)
                {
                    using(var stream = new MemoryStream())
                    {
                        model.BannerImage.CopyTo(stream);
                        movie.Banner = stream.ToArray();
                    }
                }

                context.Movies.Add(movie);
                await context.SaveChangesAsync();

                return Ok();
            }
            else 
            {
                ViewData["Title"] = "Добавление нового фильма";
                ViewData["BannerBtn"] = "Добавить постер";
                ViewData["Action"] = "Create"; 
                return PartialView("_EditPartial", model);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var movie = await context.Movies
                .Include(m => m.User)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
            
            if(movie != null && User.Identity.Name == movie.User.UserName)
            {
                var model = new MovieEditModel() {
                    Id = movie.Id.ToString(), 
                    Name = movie.Name, 
                    ProductionYear = movie.ProductionYear, 
                    Director = movie.Director, 
                    Description = movie.Description
                };                        
            
                ViewData["Title"] = "Редактирование фильма";
                ViewData["BannerBtn"] = "Заменить постер";
                ViewData["Action"] = "Edit"; 
                return PartialView("_EditPartial", model);
            }
            else
            {
                return NotFound();
            }
            
        }
        
        [HttpPost]
        public async Task<ActionResult> Edit(MovieEditModel model)
        {
            if(ModelState.IsValid)
            {
                var movie = await context.Movies
                    .Include(m => m.User)
                    .Where(m => m.Id == Int32.Parse(model.Id))
                    .FirstOrDefaultAsync();
                
                if(movie != null && User.Identity.Name == movie.User.UserName)
                {
                    movie.Name = model.Name;
                    movie.ProductionYear = model.ProductionYear ?? 0; 
                    movie.Director = model.Director; 
                    movie.Description = model.Description;

                    if(model.BannerImage != null)
                    {
                        using(var stream = new MemoryStream())
                        {
                            model.BannerImage.CopyTo(stream);
                            movie.Banner = stream.ToArray();
                        }
                    }

                    context.Entry(movie).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                ViewData["Title"] = "Редактирование фильма";
                ViewData["BannerBtn"] = "Заменить постер";
                ViewData["Action"] = "Edit"; 
                return PartialView("_EditPartial", model);
            }
        }
    }
}