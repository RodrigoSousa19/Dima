using Dima.Api.Data;
using Dima.Api.Handlers;
using Dima.Api.Models;
using Dima.Core;
using Dima.Core.Handlers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Dima.Api.Common.Api
{
    public static class BuilderExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            Configurations.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            Configurations.BackEndUrl = builder.Configuration.GetValue<string>("BackEndUrl") ?? string.Empty;
            Configurations.FrontEndUrl = builder.Configuration.GetValue<string>("FrontEndUrl") ?? string.Empty;
            ApiConfiguration.StripeApiKey = builder.Configuration.GetValue<string>("StripeApiKey") ?? string.Empty;

            StripeConfiguration.ApiKey = ApiConfiguration.StripeApiKey;
        }

        public static void AddDocumentations(this WebApplicationBuilder builder)
        {
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(n => n.FullName);
            });
        }

        public static void AddSecurity(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                            .AddIdentityCookies();
            builder.Services.AddAuthorization();
        }

        public static void AddDataContexts(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(
                x =>
                {
                    x.UseSqlServer(Configurations.ConnectionString);
                });

            builder.Services.AddIdentityCore<User>()
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();
        }

        public static void AddCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(
                options => options
                .AddPolicy(ApiConfiguration.CorsPolicyName,
                policy => policy
                .WithOrigins([Configurations.BackEndUrl, Configurations.FrontEndUrl])
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials())
                );
        }

        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
            builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
            builder.Services.AddTransient<IReportHandler, ReportHandler>();
            builder.Services.AddTransient<IOrderHandler, OrderHandler>();
            builder.Services.AddTransient<IStripeHandler, StripeHandler>();
        }
    }
}
