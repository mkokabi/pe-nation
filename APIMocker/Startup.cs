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

        public static List<APIDetailOptions> APIDetailOptions { get; set; } = new List<APIDetailOptions>();

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
                var match = APIDetailOptions.FirstOrDefault(opt =>
                    path.Equals(opt.Path, StringComparison.OrdinalIgnoreCase) &&
                    QueryStringChecker.Match(opt.QueryString, queryString));
                if (match != null)
                {
                    response.StatusCode = match.StatusCode;
                    var stream = response.Body;
                    await stream.WriteAsync(Encoding.UTF8.GetBytes(match.ResponseBody));
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
