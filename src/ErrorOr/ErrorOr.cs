using System.Diagnostics.CodeAnalysis;

namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    public ErrorOr()
    {
        throw new InvalidOperationException("Default construction of ErrorOr<TValue> is invalid. Please use provided factory methods to instantiate.");
    }

    private ErrorOr(params IReadOnlyList<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (errors.Count == 0)
        {
            throw new ArgumentException("Cannot create an ErrorOr<TValue> from an empty collection of errors. Provide at least one error.", nameof(errors));
        }

        Errors = errors;
    }

    private ErrorOr(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        Errors = [];
    }

    public IReadOnlyList<Error> Errors { get; }

    public Error FirstError => IsError ? Errors[0] : default;

    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(FirstError))]
    public bool IsError => Errors.Count > 0;

    public TValue? Value { get; }

    public static ErrorOr<TValue> From(IReadOnlyList<Error> errors)
    {
        return new(errors);
    }
}
