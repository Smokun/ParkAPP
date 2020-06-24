using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkAPI
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

      
        public void Configure(SwaggerGenOptions options)
        {   
            foreach(var desc in provider.ApiVersionDescriptions)
            {   //going via loop via all the api versions
                options.SwaggerDoc(desc.GroupName, new OpenApiInfo()
                {
                    Title = $"ParkAPI {desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString()
                } );
            }
        }
    }
}
