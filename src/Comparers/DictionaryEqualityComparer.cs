using System.Diagnostics.CodeAnalysis;

namespace ErrorOr;

/// <summary>
/// Provides an equality comparer for <see cref="IReadOnlyDictionary{TKey, TValue}"/> instances
/// that compares dictionaries by their key-value pairs in an order-independent manner.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
/// <param name="valueComparer">
/// An optional equality comparer for values. If <c>null</c>, <see cref="EqualityComparer{T}.Default"/> is used.
/// </param>
/// <remarks>
/// Two dictionaries are considered equal if they have the same count and contain the same key-value pairs,
/// regardless of the order in which the pairs are enumerated.
/// </remarks>
internal sealed class DictionaryEqualityComparer<TKey, TValue>(
    IEqualityComparer<TValue>? valueComparer = null)
    : IEqualityComparer<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    private readonly IEqualityComparer<TValue> _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

    /// <summary>
    /// Gets the default instance of <see cref="DictionaryEqualityComparer{TKey, TValue}"/>
    /// using the default equality comparers for keys and values.
    /// </summary>
    public static DictionaryEqualityComparer<TKey, TValue> Default { get; } = new();

    /// <inheritdoc />
    public bool Equals(IReadOnlyDictionary<TKey, TValue>? x, IReadOnlyDictionary<TKey, TValue>? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null || x.Count != y.Count)
        {
            return false;
        }

        foreach (var pair in x)
        {
            if (!y.TryGetValue(pair.Key, out var otherValue) ||
                !_valueComparer.Equals(pair.Value, otherValue))
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc />
    public int GetHashCode([DisallowNull] IReadOnlyDictionary<TKey, TValue> obj)
    {
        var keysHash = 0;
        var valuesHash = 0;

        foreach (var pair in obj)
        {
            keysHash ^= pair.Key.GetHashCode();
            valuesHash ^= pair.Value is null
                ? 0
                : _valueComparer.GetHashCode(pair.Value);
        }

        return HashCode.Combine(obj.Count, keysHash, valuesHash);
    }
}
