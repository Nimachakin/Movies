using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name="Описание")]
        public string Description { get; set; }
        [Display(Name="Год издания")]
        public int ProductionYear { get; set; }
        [Display(Name="Режиссер")]
        public string Director { get; set; }
        [Display(Name="Постер")]
        public byte[] Banner { get; set; }
        [Required]
        public User User { get; set; }
    }
}