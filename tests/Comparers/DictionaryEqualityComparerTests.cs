namespace Tests;

public class DictionaryEqualityComparerTests
{
    [Fact]
    public void Equals_SameReference_ReturnsTrue()
    {
        var dict = new Dictionary<string, int> { ["a"] = 1 };
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(dict, dict);

        result.ShouldBeTrue();
    }

    [Fact]
    public void Equals_BothNull_ReturnsTrue()
    {
        IReadOnlyDictionary<string, int>? x = null;
        IReadOnlyDictionary<string, int>? y = null;
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeTrue();
    }

    [Fact]
    public void Equals_OneNull_ReturnsFalse()
    {
        IReadOnlyDictionary<string, int>? x = null;
        IReadOnlyDictionary<string, int>? y = new Dictionary<string, int>();
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_DifferentCount_ReturnsFalse()
    {
        var x = new Dictionary<string, int> { ["a"] = 1 };
        var y = new Dictionary<string, int>();
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_SamePairsDifferentOrder_ReturnsTrue()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        IReadOnlyDictionary<string, int> y = new Dictionary<string, int>
        {
            ["b"] = 2,
            ["a"] = 1,
        };

        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeTrue();
    }

    [Fact]
    public void Equals_DifferentValuesForSameKey_ReturnsFalse()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int> { ["a"] = 1 };
        IReadOnlyDictionary<string, int> y = new Dictionary<string, int> { ["a"] = 2 };
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_DifferentKeys_ReturnsFalse()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int> { ["a"] = 1 };
        IReadOnlyDictionary<string, int> y = new Dictionary<string, int> { ["b"] = 1 };
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var result = comparer.Equals(x, y);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_DifferentCasingKeys_ReturnsFalse()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int> { ["a"] = 1 };
        IReadOnlyDictionary<string, int> y = new Dictionary<string, int> { ["A"] = 1 };
        var comparer = new DictionaryEqualityComparer<string, int>();

        var result = comparer.Equals(x, y);

        result.ShouldBeFalse();
    }

    [Fact]
    public void Equals_UsesCustomValueComparer()
    {
        IReadOnlyDictionary<string, string> x = new Dictionary<string, string> { ["a"] = "value" };
        IReadOnlyDictionary<string, string> y = new Dictionary<string, string> { ["a"] = "VALUE" };
        var comparer = new DictionaryEqualityComparer<string, string>(StringComparer.OrdinalIgnoreCase);

        var result = comparer.Equals(x, y);

        result.ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_EqualDictionariesSameOrder_ProducesSameHashCode()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        IReadOnlyDictionary<string, int> y = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var hashX = comparer.GetHashCode(x);
        var hashY = comparer.GetHashCode(y);

        hashX.ShouldBe(hashY);
    }

    [Fact]
    public void GetHashCode_EqualDictionariesDifferentOrder_ProducesSameHashCode()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int>
        {
            ["a"] = 1,
            ["b"] = 2,
        };

        IReadOnlyDictionary<string, int> y = new Dictionary<string, int>
        {
            ["b"] = 2,
            ["a"] = 1,
        };

        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var hashX = comparer.GetHashCode(x);
        var hashY = comparer.GetHashCode(y);

        hashX.ShouldBe(hashY);
    }

    [Fact]
    public void GetHashCode_DifferentDictionaries_ProducesDifferentHashCodes()
    {
        IReadOnlyDictionary<string, int> x = new Dictionary<string, int> { ["a"] = 1 };
        IReadOnlyDictionary<string, int> y = new Dictionary<string, int> { ["a"] = 2 };
        var comparer = DictionaryEqualityComparer<string, int>.Default;

        var hashX = comparer.GetHashCode(x);
        var hashY = comparer.GetHashCode(y);

        hashX.ShouldNotBe(hashY);
    }

    [Fact]
    public void GetHashCode_HandlesNullValues()
    {
        IReadOnlyDictionary<string, string?> dict = new Dictionary<string, string?>
        {
            ["a"] = null,
            ["b"] = "value",
        };

        var comparer = new DictionaryEqualityComparer<string, string?>();

        Should.NotThrow(() => comparer.GetHashCode(dict));
    }
}
