using BusinessObject.Models;
using DataAccess.Repositories;
using eStoreAPI.AutoMapping;
using Microsoft.EntityFrameworkCore;

namespace eStoreAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<EStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")),
                ServiceLifetime.Singleton);

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins(builder.Configuration["Client"])
                                            .AllowAnyHeader()
                                            .AllowAnyMethod();
                                  });
            });
            builder.Services.AddSingleton(typeof(IRepository<>), typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(typeof(AutoMap));
            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger/OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DbContext to DI container and use SQL Server
            

            var app = builder.Build();
            app.UseCors(MyAllowSpecificOrigins);
            // Configure the HTTP request pipeline for development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
