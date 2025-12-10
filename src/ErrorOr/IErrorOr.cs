using System.Collections.Immutable;

namespace ErrorOr;

public interface IErrorOr<out TValue> : IErrorOr
{
    /// <summary>Gets the value when successful; <c>null</c> when error.</summary>
    TValue? Value { get; }
}

/// <summary>Type-less interface for the <see cref="ErrorOr"/> object.</summary>
/// <remarks>This interface is intended for use when the underlying type of the <see cref="ErrorOr"/> object is unknown.</remarks>
public interface IErrorOr
{
    /// <summary>Gets the errors when in the error state; empty when successful.</summary>
    ImmutableArray<Error> Errors { get; }

    /// <summary>Gets a value indicating whether the result contains errors.</summary>
    bool IsError { get; }
}
