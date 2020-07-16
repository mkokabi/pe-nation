using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APIMocker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static APIDetailOptions APIDetailOptions { get; set; } = new APIDetailOptions();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<APIDetailOptions>(
                Configuration.GetSection("APIDetail"));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Configuration.GetSection("APIDetail").Bind(APIDetailOptions);

            app.UseStatusCodePages(async context =>
            {
                var request = context.HttpContext.Request;
                var path = request.Path.Value;
                var queryString = request.QueryString.Value;
                var response = context.HttpContext.Response;
                if (path.Equals(APIDetailOptions.Path, StringComparison.OrdinalIgnoreCase) &&
                    queryString.Equals(APIDetailOptions.QueryString, StringComparison.OrdinalIgnoreCase))
                {
                    response.StatusCode = APIDetailOptions.StatusCode;
                    var stream = response.Body;
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(APIDetailOptions.ResponseBody));
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
