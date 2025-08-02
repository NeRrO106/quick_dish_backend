
using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
using QUickDish.API.Repos;
using QUickDish.API.Services;

namespace QUickDish.API
{
    public class Startup
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            //repositories
            services.AddScoped<UserRepo>();
            services.AddScoped<ProductRepo>();

            //services
            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();

            //database context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source = quick_dish.db"));


            //cors
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });

            });

            //auth
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.LoginPath = "/login";
                    options.Cookie.Name = "AuthCookie";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
        }
    }
}
