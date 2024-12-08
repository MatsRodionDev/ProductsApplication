using FluentValidation;
using MediatR;
using System.Text.Json;
using ProductsService.Domain.Exceptions;
using System.Runtime.InteropServices;

namespace ProductsService.Application.Common.Behaviors
{
    public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if(!_validators.Any())
            {
                return await next();
            }

            var errorMessages = _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => failure.ErrorMessage).ToArray();

            if(errorMessages.Length != 0)
            {
                throw new ValidationRequestException(errorMessages);
            }
            
            return await next();
        }
    }
}
