
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MsBanking.Common.Dto;
using MsBanking.Core.Account.Apis;
using MsBanking.Core.Account.DomainAccount;
using MsBanking.Core.Account.Services;

namespace MsBanking.Core.Account
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AccountDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddAutoMapper(typeof(AccountDtoProfile));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGroup("/api/v1/")
                .WithTags("Core Banking Api v1")
                .MapAccountApi();
           

            app.UseHttpsRedirection();

            app.UseAuthorization();

            DataSeeder.Seed(app);

            app.Run();
        }
    }
}
