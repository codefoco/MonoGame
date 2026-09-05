#if !NETSTANDARD2_0_OR_GREATER && !NETSTANDARD2_0 && !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER && !NETCOREAPP && !NET5_0_OR_GREATER && !NET6_0_OR_GREATER && !NET7_0_OR_GREATER && !NET8_0_OR_GREATER && !NET9_0_OR_GREATER

namespace System
{
    using System.Collections.Generic;

    internal struct ValueTuple<T1, T2> : IEquatable<ValueTuple<T1, T2>>, IComparable<ValueTuple<T1, T2>>, IComparable
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override bool Equals(object obj) => obj is ValueTuple<T1, T2> other && Equals(other);

        public bool Equals(ValueTuple<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<T2>.Default.Equals(Item2, other.Item2);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Item1?.GetHashCode() ?? 0);
            hash = hash * 23 + (Item2?.GetHashCode() ?? 0);
            return hash;
        }

        public override string ToString() => $"({Item1?.ToString() ?? ""}, {Item2?.ToString() ?? ""})";

        public int CompareTo(ValueTuple<T1, T2> other)
        {
            int c = Comparer<T1>.Default.Compare(Item1, other.Item1);
            if (c != 0) return c;
            return Comparer<T2>.Default.Compare(Item2, other.Item2);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is ValueTuple<T1, T2>)) throw new ArgumentException("Argument must be of type ValueTuple.", nameof(obj));
            return CompareTo((ValueTuple<T1, T2>)obj);
        }
    }

    internal struct ValueTuple<T1, T2, T3, T4> : IEquatable<ValueTuple<T1, T2, T3, T4>>, IComparable<ValueTuple<T1, T2, T3, T4>>, IComparable
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }

        public override bool Equals(object obj) => obj is ValueTuple<T1, T2, T3, T4> other && Equals(other);

        public bool Equals(ValueTuple<T1, T2, T3, T4> other)
        {
            return EqualityComparer<T1>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<T2>.Default.Equals(Item2, other.Item2) &&
                   EqualityComparer<T3>.Default.Equals(Item3, other.Item3) &&
                   EqualityComparer<T4>.Default.Equals(Item4, other.Item4);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Item1?.GetHashCode() ?? 0);
            hash = hash * 23 + (Item2?.GetHashCode() ?? 0);
            hash = hash * 23 + (Item3?.GetHashCode() ?? 0);
            hash = hash * 23 + (Item4?.GetHashCode() ?? 0);
            return hash;
        }

        public override string ToString() => $"({Item1?.ToString() ?? ""}, {Item2?.ToString() ?? ""}, {Item3?.ToString() ?? ""}, {Item4?.ToString() ?? ""})";

        public int CompareTo(ValueTuple<T1, T2, T3, T4> other)
        {
            int c = Comparer<T1>.Default.Compare(Item1, other.Item1);
            if (c != 0) return c;
            c = Comparer<T2>.Default.Compare(Item2, other.Item2);
            if (c != 0) return c;
            c = Comparer<T3>.Default.Compare(Item3, other.Item3);
            if (c != 0) return c;
            return Comparer<T4>.Default.Compare(Item4, other.Item4);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) return 1;
            if (!(obj is ValueTuple<T1, T2, T3, T4>)) throw new ArgumentException("Argument must be of type ValueTuple.", nameof(obj));
            return CompareTo((ValueTuple<T1, T2, T3, T4>)obj);
        }
    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue | AttributeTargets.Class | AttributeTargets.Struct)]
    internal sealed class TupleElementNamesAttribute : Attribute
    {
        private readonly string[] _transformNames;

        public TupleElementNamesAttribute(string[] transformNames)
        {
            _transformNames = transformNames ?? throw new ArgumentNullException(nameof(transformNames));
        }

        public System.Collections.Generic.IList<string> TransformNames => _transformNames;
    }
}
#endif