using Microsoft.EntityFrameworkCore;
using MondialExpenses.Models;

namespace MondialExpenses.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Cashier> Cashiers { get; set; }
        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cashier>()
                .HasMany(c => c.Expenses)
                .WithOne(e => e.Cashier)
                .HasForeignKey(e => e.CashierId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
