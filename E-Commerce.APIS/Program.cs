
using E_Commerce.APIS.Helpers;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Repository.Data;
using E_Commerce.Repository.Repository.Contract;
using E_Commerce.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace E_Commerce.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddIdentity<AppUser, IdentityRole>()

              .AddEntityFrameworkStores<StoreContext>();
            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty))

                };
            });

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddDistributedMemoryCache();//Register For InMemoryDistributedCache for Basket
            builder.Services.AddScoped<IBasketRepository,BasketRepository>();


            var app = builder.Build();
             using var scope=app.Services.CreateScope();
            var Services= scope.ServiceProvider;
            var _Context = Services.GetRequiredService<StoreContext>();
            var LoggerFactory= Services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _Context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_Context);//Register Seed Data 
            }
            catch (Exception ex) 
            {
                var logger=LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred during migration");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
