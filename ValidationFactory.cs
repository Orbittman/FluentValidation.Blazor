using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    public class ValidationFactory : IValidationFactory
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidationFactory(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public IValidator GetValidator(object model)
        {
            var validator = _validators.SingleOrDefault(v => v.CanValidateInstancesOfType(model.GetType())) ?? throw new InvalidOperationException($"There is no validator registered for the type {model.GetType().Name}");
            return validator;
        }
    }
}