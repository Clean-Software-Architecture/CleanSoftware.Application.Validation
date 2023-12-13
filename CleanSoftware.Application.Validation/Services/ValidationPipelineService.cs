using CleanSoftware.Application.Validation.Models;
using FluentValidation;
using MediatR;

namespace CleanSoftware.Application.Validation.Services
{
    internal class ValidationPipelineService<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineService(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var validationResusts = await Task.WhenAll(_validators
                .Select(vaidator => vaidator.ValidateAsync(request, cancellationToken))
                .ToList());

            var validationFailures = validationResusts
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (validationFailures.Any())
            {
                throw new ValidationApplicationException(validationFailures);
            }

            return await next();
        }
    }
}
