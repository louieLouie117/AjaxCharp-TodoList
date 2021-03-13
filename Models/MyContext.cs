using Microsoft.EntityFrameworkCore;

namespace AjaxCsharp.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
    }
}