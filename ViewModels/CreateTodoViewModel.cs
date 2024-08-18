using System.ComponentModel.DataAnnotations;

namespace Todo.ViewModels
{
    public class CreateTodoViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
