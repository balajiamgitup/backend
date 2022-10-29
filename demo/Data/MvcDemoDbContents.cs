using demo.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace demo.Data
{
    public class MvcDemoDbContents : DbContext
    {
        public MvcDemoDbContents(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Emp> Emp { get; set; }
    }
}
