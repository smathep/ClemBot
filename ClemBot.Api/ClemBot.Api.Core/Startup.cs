using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClemBot.Api.Data.Contexts;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serilog;

namespace ClemBot.Api.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddFluentValidation(s => {
                s.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddMediatR(typeof(Startup));

            services.AddSwaggerGen(o => {
                o.SwaggerDoc("v1", new OpenApiInfo { Title = "ClemBot.Api", Version = "1.0.0" });
                o.CustomSchemaIds(type => type.ToString());
            });

            var connectionString = Environment.GetEnvironmentVariable("CLEMBOT_CONNECTION_STRING")
                                       ?? Configuration["ClemBotConnectionString"];

            services.AddDbContext<ClemBotContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ClemBotContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClemBot.Api 1.0.0"));
            }
            app.UseSerilogRequestLogging(); // <-- Add this line

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            // Apply any new migrations
            context.Database.Migrate();

            // Reload enum types after a migration
            /*
            using var conn = (NpgsqlConnection)context.Database.GetDbConnection();
            conn.Open();
            conn.ReloadTypes();
            */
        }
    }
}
