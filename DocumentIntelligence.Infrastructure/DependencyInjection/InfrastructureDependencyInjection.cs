using System.Text;
using DocumentIntelligence.Application.Repositories;
using DocumentIntelligence.Application.Services.Auth.Interfaces;
using DocumentIntelligence.Application.Services.Users.Interfaces;
using DocumentIntelligence.Infrastructure.Persistence;
using DocumentIntelligence.Infrastructure.Repositories;
using DocumentIntelligence.Infrastructure.Services.Auth;
using DocumentIntelligence.Infrastructure.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DocumentIntelligence.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Identity context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Your main app DbContext
            services.AddDbContext<DocumentIntelligenceDbContext>(options =>
                options.UseNpgsql(connectionString));


            // Add Identity
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add custom services
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IUserAuthService,UserAuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IUserDomainRepository, UserDomainRepository>();

            // Configure JWT Authentication
            var jwtSettings = configuration.GetSection("JwtSettings");
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Secret"]))
                };
            });

            return services;
        }
    }
}
