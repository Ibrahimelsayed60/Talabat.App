using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlewares;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Repository;
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

            //builder.Services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // ModelState => Dic [KeyValuePair]
                    // Key => Name of parameter
                    // Value => Error

                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                 .SelectMany(P => P.Value.Errors)
                                 .Select(E => E.ErrorMessage)
                                 .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);

                };
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

                var dbContext = Services.GetRequiredService<StoreContext>();
                //Ask CLR For Creating Object from DbContext Explicitly
                await dbContext.Database.MigrateAsync(); //Update-Database

                await StoreContextSeed.SeedAsync(dbContext);
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
            app.UseMiddleware<ExceptionMiddleWare>();
            if (app.Environment.IsDevelopment())
            {

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
