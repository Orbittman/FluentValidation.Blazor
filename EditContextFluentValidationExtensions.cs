using Microsoft.AspNetCore.Components.Forms;

using FluentValidation;
using System;
using FluentValidation.Internal;
using System.Linq;

namespace Infrastructure
{
    public static class EditContextFluentValidationExtensions
    {
        public static EditContext AddFluentValidation(this EditContext editContext, IValidationFactory validationFactory)
        {
            if (editContext == null)
            {
                throw new ArgumentNullException(nameof(editContext));
            }

            var messages = new ValidationMessageStore(editContext);

            editContext.OnValidationRequested +=
                (sender, eventArgs) => ValidateModel((EditContext)sender, messages, validationFactory);

            editContext.OnFieldChanged +=
                (sender, eventArgs) => ValidateField(editContext, messages, eventArgs.FieldIdentifier, validationFactory);


            return editContext;
        }

        private static void ValidateField(EditContext editContext, ValidationMessageStore messages, in FieldIdentifier fieldIdentifier, IValidationFactory validationFactory)
        {
            var context = new ValidationContext(fieldIdentifier.Model, new PropertyChain(), new MemberNameValidatorSelector(new[] { fieldIdentifier.FieldName }));

            var validator = validationFactory.GetValidator(fieldIdentifier.Model);
            var validationResult = validator.Validate(context);

            messages.Clear(fieldIdentifier);
            messages.AddRange(fieldIdentifier, validationResult.Errors.Select(error => error.ErrorMessage));

            editContext.NotifyValidationStateChanged();
        }

        private static void ValidateModel(EditContext editContext, ValidationMessageStore messages, IValidationFactory validationFactory)
        {
            var validator = validationFactory.GetValidator(editContext.Model);
            var validationResult = validator.Validate(editContext.Model);

            messages.Clear();
            foreach (var error in validationResult.Errors)
            {
                messages.Add(editContext.Field(error.PropertyName), error.ErrorMessage);
            }

            editContext.NotifyValidationStateChanged();
        }
    }
}
