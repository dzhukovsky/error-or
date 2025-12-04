using System.Collections.Immutable;

namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue> : IErrorOr<TValue>
{
    /// <summary>Creates an <see cref="ErrorOr{TValue}"/> from a value.</summary>
    public static implicit operator ErrorOr<TValue>(TValue value) => new(value);

    /// <summary>Creates an <see cref="ErrorOr{TValue}"/> from an error.</summary>
    public static implicit operator ErrorOr<TValue>(Error error) => new(error);

    /// <summary>Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.</summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static implicit operator ErrorOr<TValue>(ImmutableArray<Error> errors)
        => ErrorOrFactory.Create<TValue>(errors);

    /// <summary>Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.</summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static implicit operator ErrorOr<TValue>(Error[] errors)
        => ErrorOrFactory.Create<TValue>(errors);

    /// <summary>Creates an <see cref="ErrorOr{TValue}"/> from a list of errors.</summary>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="errors"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="errors" /> is an empty array.</exception>
    public static implicit operator ErrorOr<TValue>(List<Error> errors)
        => ErrorOrFactory.Create<TValue>(errors);
}
