using Microsoft.EntityFrameworkCore;

namespace Employee_Management.Models
{
    public class IdentityDbContext
    {
        private DbContextOptions<AppDbContext> options;

        public IdentityDbContext(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }
    }
}