namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    public bool Equals(ErrorOr<TValue> other)
    {
        if (!IsError)
        {
            return !other.IsError && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        return other.IsError && Errors.SequenceEqual(other.Errors);
    }

    public override int GetHashCode()
    {
        if (!IsError)
        {
            return Value.GetHashCode();
        }

#pragma warning disable SA1129 // HashCode needs to be instantiated this way
        var hashCode = new HashCode();
#pragma warning restore SA1129
        for (var i = 0; i < Errors.Length; i++)
        {
            hashCode.Add(Errors[i]);
        }

        return hashCode.ToHashCode();
    }
}
