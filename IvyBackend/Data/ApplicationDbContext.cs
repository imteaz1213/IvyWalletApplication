using IvyBackend.Models;
using IvyBackend.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IvyBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; } 
        public DbSet<Budget> Budgets { get; set; }


        public DbSet<UserCurrencyDTO> UserCurrencyDTO { get; set; }  
        public DbSet<BudgetIdDTO> BudgetIdDTO { get; set; }
        public DbSet<ExpMonthDTO> ExpMonthDTO { get; set; } 
        public DbSet<ExpenseTotalDTO> ExpenseTotalDTO { get; set; }
        public DbSet<IncomeTotalDTO> IncomeTotalDTO { get; set; }
        public DbSet<IncMonthDTO> IncomeMonthDTO { get; set; }
        public DbSet<TransactionFilterDTO> TransactionFilterDTO { get; set; }
        public DbSet<ExpenseDTO> ExpenseDTO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserCurrencyDTO>().HasNoKey();
            modelBuilder.Entity<BudgetIdDTO>().HasNoKey();
            modelBuilder.Entity<ExpMonthDTO>().HasNoKey();  
            modelBuilder.Entity<ExpenseTotalDTO>().HasNoKey();
            modelBuilder.Entity<IncomeTotalDTO>().HasNoKey();
            modelBuilder.Entity<IncMonthDTO>().HasNoKey();
            modelBuilder.Entity<IncomeTotalDTO>().HasNoKey();
            modelBuilder.Entity<TransactionFilterDTO>().HasNoKey(); 
            modelBuilder.Entity<ExpenseDTO>().HasNoKey();   
        }
    }
}


