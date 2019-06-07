using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controller
{
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        public TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;

            if (this.context.toDoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                this.context.toDoItems.Add(new TodoItem { id = 1, status = false, task = "workout" });
                this.context.toDoItems.Add(new TodoItem { id = 2, status = false, task = "code" });
                this.context.toDoItems.Add(new TodoItem { id = 3, status = false, task = "sleep" });
                this.context.SaveChanges();
            }
        }

        //GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Get()
        {
            return await context.toDoItems.ToListAsync();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(int id)
        {
            TodoItem temp = await context.toDoItems.FindAsync(id);

            if (temp == null)
            {
                return NotFound("The item: " + id + " does not exist!");
            }

            return temp;
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult<TodoItem>> Post([FromBody] TodoItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.toDoItems.Add(item);
            await context.SaveChangesAsync();

            return CreatedAtAction("get", new { item.id}, item);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TodoItem item)
        {
            if(id != item.id)
            {
                return BadRequest("Ids doesn't match");
            }
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            TodoItem temp = await context.toDoItems.FindAsync(id);
            if(temp == null)
            {
                return NotFound("Object not found, Id: " + id);
            }
            context.Remove(temp);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
