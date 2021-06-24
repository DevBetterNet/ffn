using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Plugin.Sys.Auth.Services;
using Dev.Services.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace Dev.Plugin.Sys.Auth.Infrastructure;

public class DependencyRegistrar : IDependencyRegistrar
{
    public int Order => 50;

    public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
    {
        services.AddScoped<IAuthenticationService, JWTAuthenticationService>();
        services.AddScoped<IUserService, UserService>();
    }
}
