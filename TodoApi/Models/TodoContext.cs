using Microsoft.EntityFrameworkCore;
// You can put models classes ANYWHERE in the project, but by conventions we put in it folder Models
namespace TodoApi.Models
{
    public class TodoContext: DbContext{
        public TodoContext(DbContextOptions<TodoContext> options): base(options){
        }
        public DbSet<TodoItem> TodoItems{get;set;}
    }
}