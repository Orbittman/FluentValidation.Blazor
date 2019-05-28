using Infrastructure;
using System;

namespace Microsoft.AspNetCore.Components.Forms
{
    public class FluentValidationValidator : ComponentBase
    {
        [Inject]
        private IValidationFactory ValidationFactory { get; set; }

        [CascadingParameter] EditContext CurrentEditContext { get; set; }

        protected override void OnInit()
        {
            if (ValidationFactory == null)
            {
                throw new InvalidOperationException($"{nameof(FluentValidationValidator)} requires a validation factory");
            }

            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(FluentValidationValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(FluentValidationValidator)} " +
                    $"inside an {nameof(EditForm)}.");
            }

            CurrentEditContext.AddFluentValidation(ValidationFactory);
        }
    }
}