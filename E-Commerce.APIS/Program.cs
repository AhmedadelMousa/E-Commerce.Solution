using E_Commerce.APIS.Helpers;
using E_Commerce.Core.Entities.Identity;
using E_Commerce.Core.Repositories.Contract;
using E_Commerce.Core.Services.Contract;
using E_Commerce.Repository.Data;
using E_Commerce.Repository.Repository.Contract;
using E_Commerce.Service;
using E_Commerce.Service.Helpers;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}j

                    }
                });
            });
            builder.Services.Configure<AppBaseLinks>(builder.Configuration.GetSection(nameof(AppBaseLinks)));
            builder.Services.Configure<JwtConfigurations>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddDbContext<StoreContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));
            builder.Services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreContext>()
                .AddSignInManager();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                    context.Response.Redirect(context.RedirectUri);
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddAuthentication(options =>
            {
                // ? Tell ASP.NET Core to use JWT Bearer by default
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        //ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidateAudience = false,
                        //ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty))

                    };
                });
            builder.Services.AddAuthorization();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDevClient", policy =>
                {
                    policy
                      //.WithOrigins("https://4zkpd097-4200.euw.devtunnels.ms")   // your Angular dev server
                      .AllowAnyOrigin()                        // allow any origin
                      .AllowAnyMethod()                       // GET, POST, OPTIONS, etc.
                      .AllowAnyHeader();                       // Content-Type, Authorization, etc.
                });
            });

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddDistributedMemoryCache();//Register For InMemoryDistributedCache for Basket
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();


            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;
            var _Context = Services.GetRequiredService<StoreContext>();
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _Context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(_Context);//Register Seed Data 
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred during migration");
            }

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            // }

            app.UseRouting();
            app.UseCors("AllowAngularDevClient");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
