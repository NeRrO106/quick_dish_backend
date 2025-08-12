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

            services.AddMemoryCache();

            services.AddScoped<UserRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<OrderRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<ProductService>();
            services.AddScoped<OrderService>();
            services.AddScoped<AuthService>();

            services.AddSingleton<EmailService>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source = quick_dish.db"));

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

            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.Cookie.Name = "CookieAuth";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = false;
                });
        }
    }
}
