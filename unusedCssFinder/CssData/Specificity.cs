using System;

namespace UnusedCssFinder.CssData
{
    public class Specificity : IComparable<Specificity>
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

        protected bool Equals(Specificity other)
        {
            if (other == null)
                return false;

            return other.A == A && other.B == B && other.C == C && other.D == D;
        }

        public static bool operator ==(Specificity x, Specificity y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Specificity x, Specificity y)
        {
            return !x.Equals(y);
        }

        public static bool operator >(Specificity x, Specificity y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator <(Specificity x, Specificity y)
        {
            return x.CompareTo(y) < 0;
        }
    }
}