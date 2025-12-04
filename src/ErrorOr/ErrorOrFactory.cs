using System.Collections.Immutable;

namespace ErrorOr;

/// <summary>Provides factory methods for creating instances of <see cref="ErrorOr{TValue}"/>.</summary>
public static class ErrorOrFactory
{
    /// <summary>Create a successful result from the specified value.</summary>
    public static ErrorOr<TValue> Create<TValue>(TValue value) => new(value);

    /// <summary>Create an error result from the specified non-empty error list.</summary>
    /// <exception cref="ArgumentException">List is empty.</exception>
    public static ErrorOr<TValue> Create<TValue>(ImmutableArray<Error> errors) => new(errors);

    /// <summary>Create an error result from the specified non-empty error list.</summary>
    /// <exception cref="ArgumentNullException">List is null.</exception>
    /// <exception cref="ArgumentException">List is empty.</exception>
    public static ErrorOr<TValue> Create<TValue>(IReadOnlyList<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        return new([.. errors]);
    }
}
