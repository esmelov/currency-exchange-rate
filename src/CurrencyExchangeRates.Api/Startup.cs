using Cbr.Client;
using Cbr.Client.Abstractions;
using Cbr.Client.Contracts;
using CurrencyExchangeRates.Api.DAL;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

namespace CurrencyExchangeRates.Api
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(x =>
                    x.RegisterValidatorsFromAssembly(typeof(Startup).Assembly, lifetime: ServiceLifetime.Singleton));

            services.AddMediatR(typeof(Startup));

            services.AddMemoryCache(x => x.ExpirationScanFrequency = TimeSpan.FromDays(1));

            services.Configure<CbrClientConfiguration>(Configuration.GetSection(nameof(CbrClient)));

            services.AddHttpClient<ICbrClient, CbrClient>((sp, x) =>
            {
                var config = sp.GetRequiredService<IOptions<CbrClientConfiguration>>();
                x.BaseAddress = config.Value.BaseUrl;
                var assemblyName = Assembly.GetExecutingAssembly().GetName();
                x.DefaultRequestHeaders.UserAgent.Add(
                    new ProductInfoHeaderValue(
                        new ProductHeaderValue(assemblyName.Name,
                            assemblyName.Version.ToString())));
            });

            services.AddScoped<IRepository<CurrencyInfo>, CurrencyRepository>();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                var assembly = typeof(Startup).Assembly;
                c.IncludeXmlComments(Path.ChangeExtension(assembly.Location, ".xml"));
                c.CustomSchemaIds(type => type.ToString());
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CurrencyExchangeRates.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CurrencyExchangeRates.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
