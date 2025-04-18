using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Infrastructure.Data;
using DomusFix.Api.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NSwag.Generation.Processors.Security;
using NSwag;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

public static class DependencyInjection
{
    public static void AddWebServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddScoped<IUser, CurrentUser>();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        builder.Services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowReactApp", policy =>
            {
                policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                      .AllowCredentials();
            });
        });

        builder.Services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "DomusFix.Api API";

            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        // Load JWT settings
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new ArgumentNullException());

        var authBuilder = builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });

        // Add JWT Bearer
        authBuilder.AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        // ➕ Add Google
        authBuilder.AddGoogle(google =>
        {
            var googleConfig = builder.Configuration.GetSection("Authentication:Google");
            google.ClientId = googleConfig["ClientId"]!;
            google.ClientSecret = googleConfig["ClientSecret"]!;
        });

        // ➕ Add Apple
        authBuilder.AddApple(options =>
        {
            var appleConfig = builder.Configuration.GetSection("Authentication:Apple");

            options.ClientId = appleConfig["ClientId"] ?? throw new ArgumentNullException("ClientId");
            options.KeyId = appleConfig["KeyId"] ?? throw new ArgumentNullException("KeyId");
            options.TeamId = appleConfig["TeamId"] ?? throw new ArgumentNullException("TeamId");

            var privateKey = appleConfig["PrivateKey"];
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new Exception("Apple Private Key is missing");

            // ✅ Assign the private key directly
            options.GenerateClientSecret = true;
            options.PrivateKey = (_, _) => Task.FromResult(privateKey.AsMemory());
        });


        builder.Services.AddAuthorization();
    }
}
