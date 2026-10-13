using System.ComponentModel.DataAnnotations;

namespace TaskApi.DTOs
{
    public class CreateProjectDto
    {
        [Required]
        [StringLength(100, MinimumLength =1)]
        public string Name { get; set; } = "";

        [StringLength(200)]
        public string Description { get; set; } = "";

        public DateTime StartDate { get; set; } 
    }
}
