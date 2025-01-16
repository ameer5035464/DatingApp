using AppDate.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AppDate.Infrastructure.Persistence._Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<AppUser> Users { get; set; }
    }
}
