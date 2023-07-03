using Jynx.Common;
using Jynx.Common.ErrorHandling;
using Jynx.Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace Jynx.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

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

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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