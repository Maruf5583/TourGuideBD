using FluentValidation.Results;

namespace TourGuideBD.Domain.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    // ✅ এই constructor টা ADD করো (single field + message)
    public ValidationException(string field, string message) : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { message } }
        };
    }

    // ✅ এই constructor টা ADD করো (IEnumerable<string> errors)
    public ValidationException(IEnumerable<string> errors) : this()
    {
        Errors = new Dictionary<string, string[]>
        {
            { "General", errors.ToArray() }
        };
    }

    // আগে থেকে আছে - রাখো
    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(
                failureGroup => failureGroup.Key,
                failureGroup => failureGroup.ToArray());
    }
}