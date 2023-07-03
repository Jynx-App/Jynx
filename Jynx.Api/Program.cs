using Jynx.Common;
using Jynx.Common.AspNetCore;
using Jynx.Common.Auth;
using Jynx.Common.ErrorHandling;
using Jynx.Common.Logging;
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
                .AddCommon(builder.Configuration)
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

            app.EnableRequestBodyBuffering();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app
                    .UseSwagger()
                    .UseSwaggerUI();
            }

            app
                .AddJynxLogging(state =>
                {
                    //
                })
                .AddJynxErrorHandling(ex =>
                {
                    return false; // Break here to view caught Exceptions.
                })
                .UseHttpsRedirection()
                .UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}