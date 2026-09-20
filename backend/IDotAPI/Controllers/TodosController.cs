using Microsoft.AspNetCore.Mvc;
using IDotAPI.Models;

namespace IDotAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    // Temp in-memory list (on va ajouter DB après)
    private static List<Todo> todos = new();

    [HttpGet]
    public IActionResult GetAll() => Ok(todos);

    [HttpPost]
    public IActionResult Create([FromBody] CreateTodoRequest req)
    {
        var todo = new Todo { Id = todos.Count + 1, Title = req.Title };
        todos.Add(todo);
        return CreatedAtAction(nameof(GetAll), todo);
    }
}

public record CreateTodoRequest(string Title);