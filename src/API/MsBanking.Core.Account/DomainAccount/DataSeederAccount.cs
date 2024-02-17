using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsBanking.Common.Entity;
using MsBanking.Core.Account;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MsBanking.Core.Account.DomainAccount
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope =app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();


                context.Database.Migrate();

                context.Database.EnsureCreated();

      
               if (!context.Accounts.Any())
                {
                    var accountList = new List<MsBanking.Common.Entity.Account>()
                    {
                        new MsBanking.Common.Entity.Account
                        {
                         
                            AccountNumber = "123456789",
                            IbanNumber = "TR330006100519786457841326",
                            AccountType = "Checking",
                            Balance = 1000.00m,
                            Currency = "USD",
                            UsetId = "123456789",
                            BranchId = 1,
                            AccountSuffix = 123,
                            IsActive = true
                        },
                        new MsBanking.Common.Entity.Account
                        {
                        
                            AccountNumber = "987654321",
                            IbanNumber = "TR330006100519786457841327",
                            AccountType = "Savings", 
                            Balance = 5000.00m,
                            Currency = "EUR",
                            UsetId = "123456798",
                            BranchId = 2,
                            AccountSuffix = 456,
                            IsActive = true
                        }
                    };

                    context.Accounts.AddRange(accountList);
                    context.SaveChanges();
                }
            }
        }
    }
}
