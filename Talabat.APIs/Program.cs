using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Configure Services - Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            var app = builder.Build();

            #region Update Database
            //StoreContext dbContext = new StoreContext(); // Invalid
            //await dbContext.Database.MigrateAsync();

            using var Scope = app.Services.CreateScope();
            // Group of Services with Lifetime scoped
            var Services = Scope.ServiceProvider;
            // Services Itself

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {

                var DbContext = Services.GetRequiredService<StoreContext>();
                //Ask CLR For Creating Object from DbContext Explicitly
                await DbContext.Database.MigrateAsync(); //Update-Database
            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An error Occured during Appling The Migration");
            }

            //Scope.Dispose();

            #endregion


            // Configure the HTTP request pipeline.
            #region Configure - Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
