namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public int ExecutorId { get; set; }
        public Executor Executor { get; set; } = null!;
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "";
        public int Priority { get; set; }
        public DateTime Deadline { get; set; }
    }
}
