using Dima.Api.Models;
using Dima.Core.Models;
using Dima.Core.Reports;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Dima.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<long>, long, IdentityUserClaim<long>, IdentityUserRole<long>, IdentityUserLogin<long>, IdentityRoleClaim<long>, IdentityUserToken<long>>(options)
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<IncomesAndExpenses> IncomesAndExpenses { get; set; } = null!;
        public DbSet<IncomesByCategory> IncomesByCategories { get; set; } = null!;
        public DbSet<ExpensesByCategory> ExpensesByCategories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<IncomesAndExpenses>().HasNoKey().ToView("vw_GetIncomesAndExpenses");
            builder.Entity<IncomesByCategory>().HasNoKey().ToView("vw_GetIncomesByCategory");
            builder.Entity<ExpensesByCategory>().HasNoKey().ToView("vw_GetExpensesByCategory");
        }
    }
}
