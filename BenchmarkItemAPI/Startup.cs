using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Repository.DependencyInjection;
using Services.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Repository;

namespace BenchmarkItemAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string AllowCorsConfiguration = "AllowAll";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add sqlite db context
            services.AddDbContext<SqliteDbContext>(opt =>
                opt.UseSqlite(Configuration.GetConnectionString("sqlitecon")));

            services.AddAutoMapper(typeof(Startup));

            services.AddRepositories();

            services.AddServices();

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowCorsConfiguration,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BenchmarkItemAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BenchmarkItemAPI v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowCorsConfiguration);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
