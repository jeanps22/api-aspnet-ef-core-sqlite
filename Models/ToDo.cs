namespace Todo.Models
{
    public class ToDo
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public bool Done { get; set; }
        public DateTime Data { get; set; } = new DateTime();
    }
}
