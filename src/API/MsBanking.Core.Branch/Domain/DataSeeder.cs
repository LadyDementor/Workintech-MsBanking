using Microsoft.EntityFrameworkCore;

namespace MsBanking.Core.Branch.Domain
{
    public static class DataSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BranchDbContext>();
                if (context == null)
                    return;

                context.Database.Migrate();//*Migrate metodu, veritabanı yoksa oluşturur, varsa günceller.  //Burada hata almayı saglıyor kaldırmak lazım.<InvariantGlobalization>true</InvariantGlobalization>
                context.Database.EnsureCreated();//*Bu isimde bir tablo yoksa oluşturur

                if (!context.Branches.Any())
                {

                    var branchList = new List<MsBanking.Common.Entity.Branch>()
                    {
                        new MsBanking.Common.Entity.Branch()
                        {
                            Name = "Genel Merkez",
                            Code = 9019,
                            CityId = 34,
                            CountryId = 90
                        },
                        new MsBanking.Common.Entity.Branch()
                        {
                            Name = "Zincirlikuyu",
                            Code = 9142,
                            CityId = 34,
                            CountryId = 90
                        },
                        new MsBanking.Common.Entity.Branch()
                        {
                            Name = "Ege",
                            Code = 9080,
                            CityId = 35,
                            CountryId = 90
                        }
                    };

                    context.AddRange(branchList);
                    context.SaveChanges();
                }





            }
        
        }
    }
}
