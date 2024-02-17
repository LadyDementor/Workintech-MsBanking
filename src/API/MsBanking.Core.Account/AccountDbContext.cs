using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace MsBanking.Core.Account
{
    public class AccountDbContext:DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        {
           
        }

        public DbSet<MsBanking.Common.Entity.Account> Accounts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MsBanking.Common.Entity.Account>().ToTable("Account");
           modelBuilder.Entity<MsBanking.Common.Entity.Account>().HasQueryFilter(x => x.IsActive);//*Default olarak aktif olanları getirir.Isactive true olanları getirecek.
        
        }



    }
}
