using System;

namespace UnusedCssFinder.CssData
{
    public class Specificity : IComparable<Specificity>, IEquatable<Specificity>
    {
        public int A { get; private set; }
        public int B { get; private set; }
        public int C { get; private set; }
        public int D { get; private set; }

        public Specificity(int a, int b, int c, int d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public int CompareTo(Specificity other)
        {
            if (other == null) return 1;

            if (A.CompareTo(other.A) != 0)
            {
                return A.CompareTo(other.A);                               
            }
            if (B.CompareTo(other.B) != 0)
            {
                return B.CompareTo(other.B);
            }
            if (C.CompareTo(other.C) != 0)
            {
                return C.CompareTo(other.C);
            }
            if (D.CompareTo(other.D) != 0)
            {
                return D.CompareTo(other.D);
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Specificity)
            {
                return Equals((Specificity)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return A * 1000 + B * 100 + C * 10 + D;
            }
        }

        public bool Equals(Specificity other)
        {
             return A == other.A && B == other.B && C == other.C && D == other.D;
        }

        public static bool operator >(Specificity x, Specificity y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(Specificity x, Specificity y)
        {
            return x.CompareTo(y) < 0;
        }

        public static Specificity operator +(Specificity x, Specificity y)
        {
            return new Specificity(x.A + y.A, x.B + y.B, x.C + y.C, x.D + y.D);
        }
    }
}