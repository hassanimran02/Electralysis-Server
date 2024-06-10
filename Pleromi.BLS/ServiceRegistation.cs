using Pleromi.BLS.Helper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Pleromi.BLS.Services.Generic;
using Pleromi.BLS.Services.Sample;

namespace Pleromi.BLS
{
    public static class ServiceRegistation
    {
        public static void AddPleromiBLS(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IGenericService, GenericService>();
            services.AddTransient<ISampleService, SampleService>();
        }
    }
}
