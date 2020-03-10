using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Http;

namespace SqlCommunicatorWebAPI
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Extension method used to register services.
        /// This will help to keep our startup script clean and easy to maintain.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCorService(this IServiceCollection services)
        {
            services.AddCors()
                    .AddMvc(config =>
                        {
                            config.RespectBrowserAcceptHeader = true;
                            config.ReturnHttpNotAcceptable = true;
                        }
                    )
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .AddXmlSerializerFormatters();

            // Allow the Web API to use Windows Authentication
            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
        }

    }

}
