namespace ToDo.ViewModels
{
    public class TaskForm
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; } = null!;
        public string Priority { get; set; } = null!;

        public string Url { get; set; }

        public int Order { get; set; }
    }
}
