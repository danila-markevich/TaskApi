

namespace TaskApi.DTOs
{
    public class ExecutorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Specialty { get; set; } = "";
        public int TasksCount { get; set; } 
    }
}
