using FluentValidation;

namespace DomainModel.Validation
{
    public static class ValidationExtensions
    {
        public static async Task Validate<T>(this T model, IEnumerable<IValidator<T>> validators)
        {
            if (validators != null)
            {
                var context = new ValidationContext<T>(model);
                var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }
        }
    }
}
