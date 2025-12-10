using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ErrorOr;

/// <summary>Represents either a successful <typeparamref name="TValue"/> or one or more errors.</summary>
/// <typeparam name="TValue">The type of the successful value.</typeparam>
public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>Disabled default constructor. Use factory helpers or implicit conversions.</summary>
    /// <exception cref="InvalidOperationException">Always thrown.</exception>
    public ErrorOr()
    {
        throw new InvalidOperationException($"Default construction of ErrorOr<> is invalid. Please use provided factory methods to instantiate.");
    }

    /// <summary>Create an error result from the specified non-empty error list.</summary>
    /// <exception cref="ArgumentException">List is empty.</exception>
    internal ErrorOr(ImmutableArray<Error> errors)
    {
        if (errors.IsDefault || errors.Length == 0)
        {
            throw new ArgumentException($"Cannot create an ErrorOr<> from an empty collection of errors. Provide at least one error.", nameof(errors));
        }

        Errors = errors;
    }

    /// <summary>Create a successful result from the specified value.</summary>
    internal ErrorOr(TValue value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        Errors = [];
    }

    /// <inheritdoc/>
    public ImmutableArray<Error> Errors { get; }

    /// <summary>Gets the first error or the default value.</summary>
    public Error FirstError => IsError ? Errors[0] : default;

    /// <inheritdoc/>
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsError => Errors.Length > 0;

    /// <inheritdoc/>
    public TValue? Value { get; }
}
