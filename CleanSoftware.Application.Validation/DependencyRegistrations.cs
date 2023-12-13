using CleanSoftware.Application.Validation.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSoftware.Application.Validation
{
    public static class DependencyRegistrations
    {
        public static MediatRServiceConfiguration AddValidationBehavior(
            this MediatRServiceConfiguration configuration)
        {
            configuration.AddBehavior(
                typeof(IPipelineBehavior<,>), 
                typeof(ValidationPipelineService<,>));

            return configuration;
        }
    }
}
