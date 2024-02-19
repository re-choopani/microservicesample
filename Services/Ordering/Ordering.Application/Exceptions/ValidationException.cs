using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; set; }
        public ValidationException() : base("one more validation failures have occurred")
        {
            Errors = new Dictionary<string, string[]>();
        }
        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.GroupBy(x => x.PropertyName, e => e.ErrorMessage).ToDictionary(failerGroup => failerGroup.Key, failerGroup => failerGroup.ToArray());
        }
    }
}
