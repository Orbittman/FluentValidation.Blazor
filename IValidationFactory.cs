using FluentValidation;
using System.Linq;

namespace Infrastructure
{
    public interface IValidationFactory
    {
        IValidator GetValidator(object model);
    }
}