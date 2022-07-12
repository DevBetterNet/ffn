using Dev.Core.Configuration;
using Dev.Core.Infrastructure;
using Dev.Plugin.Sys.Auth.Configuration;
using Dev.Plugin.Sys.Auth.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Dev.Plugin.Sys.Auth.Infrastructure;

public class AuthenStartup : IDevStartup
{
    public int Order => 10;

    public void Configure(IApplicationBuilder application, IWebHostEnvironment hostEnvironment, AppSettings appSettings)
    {
        if (hostEnvironment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
            application.UseSwagger();
            application.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "All plugin Web API"));
        }

        application.UseHttpsRedirection();
        application.UseAuthentication();
        application.UseAuthorization();


        // custom jwt auth middleware
        application.UseMiddleware<JwtMiddleware>();
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration, AppSettings appSettings)
    {
        JwtConfig jwtConfig = new JwtConfig();
        if (appSettings.AdditionalData.ContainsKey("Jwt"))
        {
            jwtConfig = appSettings.AdditionalData["Jwt"].ToObject<JwtConfig>();
        }

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(jwt =>
       {
           var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);

           jwt.SaveToken = true;
           jwt.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(key),
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidateLifetime = true,
               RequireExpirationTime = false
           };
       });

        services.AddSwaggerGen(swagger =>
        {
            //This is to generate the Default UI of Swagger Documentation    
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "All plugin Web API",
                Description = "Authentication and Authorization in Web API with JWT and Swagger"
            });
            // To Enable authorization using Swagger (JWT)    
            //swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            //{
            //    Name = "Authorization",
            //    Type = SecuritySchemeType.ApiKey,
            //    Scheme = "Bearer",
            //    BearerFormat = "JWT",
            //    In = ParameterLocation.Header,
            //    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            //});
            //swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //          new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference
            //                {
            //                    Type = ReferenceType.SecurityScheme,
            //                    Id = "Bearer"
            //                }
            //            },
            //            new string[] {}

            //    }
            //});
        });


    }
}
