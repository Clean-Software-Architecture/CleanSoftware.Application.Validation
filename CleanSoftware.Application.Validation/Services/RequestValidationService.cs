using FluentValidation;

namespace CleanSoftware.Application.Validation.Services
{
    public abstract class RequestValidationService<TRequest> : AbstractValidator<TRequest>
    {
        protected RequestValidationService()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;
        }
    }
}
