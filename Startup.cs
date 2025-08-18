using Microsoft.EntityFrameworkCore;
using QUickDish.API.Data;
using QUickDish.API.Repos;
using QUickDish.API.Services;
using System.Security.Claims;

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
                app.UseSwagger(c =>
                {
                    c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
                });
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

            services.AddSingleton<LoginAttemptService>();
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
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = false;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("RequireUserRole", policy => policy.RequireClaim(ClaimTypes.Role, "Client"));
                options.AddPolicy("RequireCourierRole", policy => policy.RequireClaim(ClaimTypes.Role, "Courier"));
                options.AddPolicy("RequiredManagerRole", policy => policy.RequireClaim(ClaimTypes.Role, "Manager"));

                options.AddPolicy("RequiredAdminOrManagerOrCourierRole", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role &&
                                                    (c.Value == "Admin" ||
                                                     c.Value == "Manager" ||
                                                     c.Value == "Courier"))
                    );
                });

                options.AddPolicy("RequiredAdminOrManagerRole", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role &&
                                                    (c.Value == "Admin" ||
                                                     c.Value == "Manager"))
                    );
                });
                options.AddPolicy("RequiredAdminOrManagerOrUserRole", policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.HasClaim(c => c.Type == ClaimTypes.Role &&
                                                    (c.Value == "Admin" ||
                                                     c.Value == "Manager" ||
                                                     c.Value == "Client"))
                    );
                });
            });
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });
        }
    }
}
