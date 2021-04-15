using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Entities;
using Api.Repositories.Implementations;
using Api.Repositories.Interfaces;
using Api.Services.Implementations;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using FluentMigrator.Runner;
using Npgsql;
using System.Reflection;
using Api.Middlewares;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            var connection = new NpgsqlConnection(Configuration.GetConnectionString("PostgreSqlConnectionString"));
            services.AddControllers();

            services.AddSingleton<NpgsqlConnection>(connection);
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<ICryptographyService, CryptographyService>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                .AddPostgres()
                .WithGlobalConnectionString(Configuration.GetConnectionString("PostgreSqlConnectionString"))
                .ScanIn(Assembly.GetExecutingAssembly()).For.All()
                ).AddLogging(cfg => cfg.AddFluentMigratorConsole());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseMiddleware<AuthenticationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();
                migrator.MigrateUp();
            }
        }
    }
}
