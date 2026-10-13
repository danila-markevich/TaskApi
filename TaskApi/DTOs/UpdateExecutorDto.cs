using System.ComponentModel.DataAnnotations;


namespace TaskApi.DTOs
{
    public class UpdateExecutorDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Specialty { get; set; } = "";

    }
}