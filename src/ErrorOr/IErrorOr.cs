using System.Collections.Immutable;

namespace ErrorOr;

public interface IErrorOr<out TValue> : IErrorOr
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    TValue? Value { get; }
}

/// <summary>
/// Type-less interface for the <see cref="ErrorOr"/> object.
/// </summary>
/// <remarks>
/// This interface is intended for use when the underlying type of the <see cref="ErrorOr"/> object is unknown.
/// </remarks>
public interface IErrorOr
{
    /// <summary>
    /// Gets the collection of errors.
    /// </summary>
    ImmutableArray<Error> Errors { get; }

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    bool IsError { get; }
}
