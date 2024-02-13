
using MsBanking.Core.Apis;
using MsBanking.Core.Domain;
using MsBanking.Core.Services;

namespace MsBanking.Core
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

            //*DatabaseOption sýnýfýný ekliyoruz.Configure metodu ile appsettings.json dosyasýndaki DatabaseOption alanýný okuyoruz.
            builder.Services.Configure<DatabaseOption>(builder.Configuration.GetSection("DatabaseOption"));

            builder.Services.AddScoped<ICustomerService, CustomerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            app.MapGroup("/api/v1/")
                .WithTags("Core Banking Api v1")
                .MapCustomerApi();


            app.UseHttpsRedirection();

            app.UseAuthorization();

         
         

            app.Run();
        }
    }
}
