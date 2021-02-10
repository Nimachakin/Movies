using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MovieCatalog.Models
{
    public class MovieEditModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage="Укажите название фильма")]
        [Display(Name="Название")]
        public string Name { get; set; }

        [Display(Name="Описание")]
        public string Description { get; set; }

        [Display(Name="Год выхода")]
        public int? ProductionYear { get; set; }

        [Display(Name="Режиссер")]
        public string Director { get; set; }

        [Display(Name="Постер")]
        public IFormFile BannerImage { get; set; }
    }
}