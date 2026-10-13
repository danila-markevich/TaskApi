namespace TaskApi.DTOs
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; } 
        public int ExecutorId { get; set; }
        public string? ExecutorName { get; set; } 
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }
    }
}
