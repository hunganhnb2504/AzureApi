using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; }
    public DbSet<ToDo> ToDo => Set <ToDo> ();
}