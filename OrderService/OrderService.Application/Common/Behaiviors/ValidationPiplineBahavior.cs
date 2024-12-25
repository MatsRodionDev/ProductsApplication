using FluentValidation;
using MediatR;
using OrderService.Domain.Exceptions;
using System.Text;

namespace OrderService.Application.Common.Behaiviors
{
    internal sealed class ValidationPiplineBahavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (!validators.Any())
            {
                return await next();
            }

            var errorMessages = validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => failure.ErrorMessage).ToArray();

            var errors = new StringBuilder();

            foreach(var errorMessage in errorMessages ) 
            {
                errors.AppendLine(errorMessage);
            }

            if (errors.Length != 0)
            {
                throw new ValidationRequestException(errors.ToString());
            }

            return await next();
        }
    }
}
