using FluentValidation.Results;
using System.Text;

namespace CleanSoftware.Application.Validation.Models
{
    public class ValidationApplicationException : Exception
    {
        private readonly Lazy<List<ValidationFailure>> _failures;

        public ValidationApplicationException(IReadOnlyCollection<ValidationFailure> failures)
            : this()
        {
            _failures = new Lazy<List<ValidationFailure>>(failures.ToList());
        }

        public ValidationApplicationException(
            IReadOnlyCollection<(string PropertyName, string ErrorMessage)> failures)
            : this()
        {
            _failures = new Lazy<List<ValidationFailure>>(
                failures
                    .Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage))
                    .ToList());
        }

        public ValidationApplicationException(string propertyName, string errorMessage)
            : this()
        {
            _failures = new Lazy<List<ValidationFailure>>(
                new List<ValidationFailure> { new ValidationFailure(propertyName, errorMessage) });
        }

        public ValidationApplicationException(string errorMessage)
            : base(errorMessage)
        {
            _failures = new Lazy<List<ValidationFailure>>();
        }

        public ValidationApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ValidationApplicationException()
            : base("One or more validation failures have occurred")
        {
            _failures = new Lazy<List<ValidationFailure>>();
        }

        public IReadOnlyCollection<ValidationFailure> Failures => _failures.Value;

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Failures.Count > 0)
            {
                builder.AppendLine($"{nameof(Failures)}: {Failures.Count}");
                builder.AppendLine();

                for (int i = 0; i < Failures.Count; i++)
                {
                    var failure = Failures.ElementAt(i);

                    builder.AppendLine($"-PropertyName: {failure.PropertyName}");
                    builder.AppendLine($"-AttemptedValue: {failure.AttemptedValue}");
                    builder.AppendLine($"-ErrorMessage: {failure.ErrorMessage}");
                    builder.AppendLine();
                }
            }

            //builder.AppendLine(base.ToString());

            return builder.ToString();
        }
    }
}
