using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers{
    [Route("api/[controller]")] // This means [controller] will be replaced with "todo" below, yeah that's how it works.
    public class TodoController: Controller{
        private readonly TodoContext _context;
        public TodoController(TodoContext context){
            _context = context;
            if(_context.TodoItems.Count() == 0){
                _context.TodoItems.Add(new TodoItem{Name = "Item1"});
                _context.SaveChanges();
            }

            // Response code is 200.
            // If something WRONG, response code is 5xx errors (SERVER side errors)
        }

        [HttpGet("/products")] // --> So you can use something like : localhost:5000/products FOR GET METHOD
        public IEnumerable<TodoItem> GetAll(){
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")] // id is THE PLACEHOLDER variable for the ID of the todo item.
                                            // if you get the VALUE of id into the method's you will get it back.
                                            // Name = "GetTodo" CREATE of name route.
        public IActionResult GetById(long id){
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(item == null){
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item){ // FromBody tells MVC to get the value of the to-do item from the body of the HTTP request)
            if(item ==null){
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetTodo", new{id = item.Id}, item);
            // Create at route:
            // - return 201 response. HTTP 201 is the standard response for an HTTP POST method that creates a new resource on the server.
            // - Adds a Location header to the response. The Location header specifies the URI of the newly created to-do item.
            // - Uses the "GetTodo" named route to create the URL. The "GetTodo" named route is defined in GetById.
        }

        [HttpPut("{id}")] // Response 204 (No Content)
                          // PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH
        public IActionResult Update(long id, [FromBody] TodoItem item){
            if (item != null || item.Id != id){
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if(todo == null){
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;
            
            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("id")]
        public IActionResult Delete(long id){
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null){
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }


    }
}