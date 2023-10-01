﻿using FluentValidation;
using MediatR;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace MyWeatherServer.Pipeline;

public class ValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipeline(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var validationFailures = _validators
                                 .Select(validator => validator.Validate(request))
                                 .SelectMany(validationResult => validationResult.Errors)
                                 .Where(validationFailure => validationFailure != null)
                                 .ToList();

        if (!validationFailures.Any())
        {
            return next();
        }

        var error = string.Join("\r\n", validationFailures);

        throw new ValidationException(error);
    }
}