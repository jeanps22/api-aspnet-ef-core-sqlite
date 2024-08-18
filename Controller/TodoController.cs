using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Models;
using Todo.ViewModels;

namespace Todo.Controllers
{
    [ApiController]
    [Route("v1")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route("todos/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromServices] AppDbContext context, [FromRoute] int id)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return todos == null ? NotFound() : Ok(todos);
        }

        [HttpPost]
        [Route("todos")]
        public async Task<IActionResult> PostAsync([FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var todo = new ToDo()
            {
                Data = DateTime.Now,
                Title = model.Title,
                Done = false
            };

            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/Create", todo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("todos/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("todos/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] AppDbContext context,
            [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if (todo == null)
                return NotFound();

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}
