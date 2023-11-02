using Insura.Media.Solusi.Models;
using Microsoft.EntityFrameworkCore;

namespace Insura.Media.Solusi.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
