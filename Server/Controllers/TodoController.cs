// TodoController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBlazor.Server.Data;
using ProjectBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TodoItem>> GetTodoItems()
    {
        return await _context.TodoItems.OrderByDescending(item => item.DateInsert).ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItem>> GetTodoById(Guid id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);

        if (todoItem == null)
        {
            return NotFound(); // Retorna um status 404 (Not Found) se o registro não for encontrado.
        }

        return todoItem;
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodoItems), new { id = todoItem.Id }, todoItem);
    }

    [HttpPut("confirm/{id}")]
    public IActionResult UpdateState(Guid Id, [FromBody] string isCompleted)
    {

        var sql = "UPDATE \"TodoItems\" SET \"Green\" = {1} WHERE \"Id\" = {0}";
        _context.Database.ExecuteSqlRaw(sql, Id, bool.Parse(isCompleted));

        // Lógica de tratamento de erro e resposta apropriada, se necessário
        return Ok(); // Retorna um status HTTP 200 OK, por exemplo
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
    {
        if (id != todoItem.Id)
        {
            return BadRequest();
        }

        _context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(Guid id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound();
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool TodoItemExists(Guid id)
    {
        return _context.TodoItems.Any(e => e.Id == id);
    }
}
