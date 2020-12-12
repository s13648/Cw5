using Cw5.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cw5
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
            services.AddSingleton<IStudyDbService, StudyDbService>();
            services.AddSingleton<IStudentDbService, StudentDbService>();
            services.AddSingleton<IEnrollmentDbService, EnrollmentDbService>();

            services.AddSingleton<IConfig,Config>(b => new Config
            {
                ConnectionString = Configuration["DbConnectionString"]
            });

            services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonDateTimeConverter()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
