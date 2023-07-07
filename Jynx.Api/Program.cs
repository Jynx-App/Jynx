using Jynx.Api.Auth;
using Jynx.Api.Security.Claims;
using Jynx.Common;
using Jynx.Common.AspNetCore;
using Jynx.Common.AspNetCore.ErrorHandling;
using Jynx.Common.AspNetCore.Logging;
using Jynx.Core;
using Jynx.Data.Cosmos;
using Jynx.Validation.Fluent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jynx.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();

            builder.Services
                .Configure<ApiBehaviorOptions>(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddApiVersioning(options =>
                {
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new QueryStringApiVersionReader("api-version"),
                        new HeaderApiVersionReader("x-api-version")
                    );
                })
                .AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                })
                // Other Projects
                .AddCommon(builder.Configuration)
                .AddCore(builder.Configuration)
                .AddCosmos(builder.Configuration)
                // PolicyProviders
                .AddSingleton<IAuthorizationPolicyProvider, RequireModerationPermissionPolicyProvider>()
                // Other
                .AddScoped<IAuthorizationHandler, RequireModerationPermissionHandler>()
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSeq();
                });

            // Auth
            builder.Services
                .AddAuthorization(options =>
                {

                })
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = ApiUserAuthenticationHandler.Schema;

                    options.AddScheme<ApiUserAuthenticationHandler>(ApiUserAuthenticationHandler.Schema, null);
                });

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app
                .EnableRequestBodyBuffering()
                .AddJynxLogging((context, state) =>
                {
                    state.Add("UserId", context.User.GetId() ?? "");
                })
                .AddJynxErrorHandling(ex =>
                {
                    return false; // Break here to view caught Exceptions.
                });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app
                    .UseSwagger()
                    .UseSwaggerUI();
            }

            app
                .UseHttpsRedirection()
                .UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}