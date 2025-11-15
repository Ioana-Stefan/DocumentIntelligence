using DocumentIntelligence.Application.Services.Users.Interfaces;
using DocumentIntelligence.Infrastructure.Services.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentIntelligence.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserAuthService, UserAuthService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            
            return services;
        }
    }
}
