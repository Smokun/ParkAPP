using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ParkAPI.Data;
using ParkAPI.Repository;
using ParkAPI.Repository.IRepository;
using AutoMapper;
using ParkAPI.ParkMapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ParkAPI
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
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            //we are adding all the mappings in ParkMappings
            services.AddAutoMapper(typeof(ParkMappings));
            //versioning support
            services.AddApiVersioning(options =>
            {//if the version is not specified use default
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            //swagger support
            //services.AddSwaggerGen(options=> {
            //    options.SwaggerDoc("ParkOpenAPISpec", 
            //        new Microsoft.OpenApi.Models.OpenApiInfo(){
            //            Title ="Park API",
            //            Version = "1",
            //            Description = "Park API project",
            //            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //            {
            //                Email = "todoemail@gmail.com",
            //                Name = "Smokun",
            //                Url = new Uri("https://www.google.com")
            //            },
            //            License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //            {
            //                Name = "MIT Licence",
            //                Url = new Uri("https://en.wikipedia.org/wiki/MIT_Licencse")
            //            }
            //        });

            //    //options.SwaggerDoc("ParkOpenAPISpecTrails",
            //    //   new Microsoft.OpenApi.Models.OpenApiInfo()
            //    //   {
            //    //       Title = "Park API Trails",
            //    //        Version = "1",
            //    //        Description = "Park API Trails project",
            //    //        Contact = new Microsoft.OpenApi.Models.OpenApiContact()
            //    //        {
            //    //            Email = "todoemail@gmail.com",
            //    //            Name = "Smokun",
            //    //            Url = new Uri("https://www.google.com")
            //    //        },
            //    //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //    //        {
            //    //            Name = "MIT Licence",
            //    //            Url = new Uri("https://en.wikipedia.org/wiki/MIT_Licencse")
            //    //        }
            //    //    });
            ////support for xml documentation using Reflection. 
            ////name of xml comments file
            //var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    //path of the xml comments file
            //    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(cmlCommentFullPath);
            //});
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            //adding swagger to the request pipeline
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
                options.RoutePrefix = "";
            });


            //support for swagger ui
            //app.UseSwaggerUI(options=>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkOpenAPISpec/swagger.json", "Park API");
            //    //options.SwaggerEndpoint("/swagger/ParkOpenAPISpecTrails/swagger.json", "Park API Trails");
            //    options.RoutePrefix = "";

            //});
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
