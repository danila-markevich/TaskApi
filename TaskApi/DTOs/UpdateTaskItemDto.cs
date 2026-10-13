using System.ComponentModel.DataAnnotations;


namespace TaskApi.DTOs
{
    public class UpdateTaskItemDto
    {
        [Range(1, int.MaxValue)]
        public int ProjectId { get; set; }

        [Range(1, int.MaxValue)]
        public int ExecutorId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Title { get; set; } = "";

        [StringLength(200, MinimumLength = 1)]
        public string Description { get; set; } = "";

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Status { get; set; } = "";

        [Range(1, 3)]
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }
    }
}