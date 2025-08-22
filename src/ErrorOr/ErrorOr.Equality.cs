namespace ErrorOr;

public readonly partial record struct ErrorOr<TValue>
{
    public bool Equals(ErrorOr<TValue> other)
    {
        if (!IsError)
        {
            return !other.IsError && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
        }

        return other.IsError && CheckIfErrorsAreEqual(Errors, other.Errors);
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
        for (var i = 0; i < Errors.Count; i++)
        {
            hashCode.Add(Errors[i]);
        }

        return hashCode.ToHashCode();
    }

    private static bool CheckIfErrorsAreEqual(IReadOnlyList<Error> errors1, IReadOnlyList<Error> errors2)
    {
        // This method is currently implemented with strict ordering in mind, so the errors
        // of the two lists need to be in the exact same order.
        // This avoids allocating a hash set. We could provide a dedicated EqualityComparer for
        // ErrorOr<TValue> when arbitrary orders should be supported.
        if (ReferenceEquals(errors1, errors2))
        {
            return true;
        }

        if (errors1.Count != errors2.Count)
        {
            return false;
        }

        for (var i = 0; i < errors1.Count; i++)
        {
            if (!errors1[i].Equals(errors2[i]))
            {
                return false;
            }
        }

        return true;
    }
}
