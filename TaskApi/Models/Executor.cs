namespace TaskApi.Models
{
    public class Executor
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Specialty { get; set; } = "";
        public List<TaskItem> Tasks { get; set; } = new();

    }
}
