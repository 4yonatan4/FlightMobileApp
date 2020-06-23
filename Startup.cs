using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Controllers;
using FlightSimulatorApp.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ISimulatorModel MySimulatorModel { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Initial the main objects of the server
            services.AddControllers();
            MySimulatorModel = new MySimulatorModel(Configuration);
            services.AddSingleton<ISimulatorModel>(MySimulatorModel);
            services.AddSingleton<CommandController>(new CommandController(MySimulatorModel));
            services.AddSingleton<ScreenshotController>(new ScreenshotController(Configuration));
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
