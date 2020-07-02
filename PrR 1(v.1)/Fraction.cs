using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace PrR_1_v._1_
{
    internal class Fraction: ICloneable, IEquatable<Fraction>,
        IComparable, IComparable<Fraction>
    {
        private BigInteger _numerator;
        private BigInteger _denominator;

        public Fraction(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0) throw new DivideByZeroException("NO GOD! PLEASE NOOOOO!!!");
            _numerator = numerator;
            _denominator = denominator;
            Reduction();
        }
        public Fraction(Fraction other)
        {
            Numerator = other.Numerator;
            Denominator = other.Denominator;
        }


        private void Reduction()
        {
            var nod = new BigInteger ();
            nod = NOD(Numerator, Denominator);
            Numerator /= nod;
            Denominator /= nod;

        }
        private static BigInteger NOD(BigInteger num1, BigInteger num2)
        {
            if (num1 == 0) return num2;
            if (num2 == 0) return num1;
            if (num1 == num2) return num1;
            if (num1 == 1 || num2 == 1) return 1;
            if ((num1 % 2 == 0) && (num2 % 2 == 0)) return 2 * NOD(num1 / 2, num2 / 2);
            if ((num1 % 2 == 0) && (num2 % 2 != 0)) return NOD(num1 / 2, num2);
            if ((num1 % 2 != 0) && (num2 % 2 == 0)) return NOD(num1, num2 / 2);
            return NOD(num2, BigInteger.Abs(num1 - num2));
        }
        public BigInteger Numerator
        {
            get =>
                _numerator;

            private set =>
                _numerator = value;
        }
        public BigInteger Denominator
        {
            get =>
                _denominator;

            private set =>
                _denominator = value;
        }

        #region ICloneable
        public object Clone()
        {
            return new Fraction(this);
        }
        #endregion

        #region IEquatable
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is Fraction frac)
            {
                return Equals(frac);
            }
            return false;
        }
        public bool Equals(Fraction frac)
        {
            if (frac is null)
            {
                return false;
            }
            return Numerator == frac.Numerator 
                && Denominator == frac.Denominator;
        }
        #endregion

        #region  IComparable
        public int CompareTo(object obj)
        {
            if(obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            if(obj is Fraction frac)
            {
                return CompareTo(frac);
            }
            throw new ArgumentException("Invalid type", nameof(obj));
        }
        public int CompareTo(Fraction frac)
        {
            if (frac is null)
            {
                throw new ArgumentNullException(nameof(frac));
            }
            Numerator *= frac.Denominator;
            frac.Numerator *= Denominator;

            var result = Numerator.CompareTo(frac.Numerator);
            Numerator /= frac.Denominator;

            return result;  
        }
        #endregion

        #region Arithmetic
        public static Fraction Pow(Fraction frac, BigInteger num)
        {
            for (int i = 0; i < num - 1; i++)
            {
                frac *= frac;
            }
            frac.Reduction();
            return frac;
        }
        private static BigInteger Sqrt(BigInteger X)
        {
            if (X < 0) throw new ArithmeticException("I can't get coplex number :c");
            const double eps = 0.01;
            double A1 = (double)X / 2;
            double A2 = 0.5*(A1 + (double)X / A1);
            while (Math.Abs(A2 - A1) > eps)
            {
                A1 = A2;
                A2 = 0.5 * (A1 + (double)X / A1);
            }
                return (BigInteger)A2;
        }
        public static Fraction Sqrt(Fraction frac)
        {
            frac.Numerator = Sqrt(frac.Numerator);
            frac.Denominator = Sqrt(frac.Denominator);
            frac.Reduction();
            return frac;
        }
        public static StringBuilder Decimal(Fraction frac, BigInteger eps)
        {
            if (eps < 0) throw new Exception("Second argument must be unsigned number");
            var result = new StringBuilder();
            var @int = new int();
            var temp = new int();
            @int = 0;
            while (frac.Numerator > frac.Denominator) {
                frac.Numerator -= frac.Denominator;
                @int++;
            }
            result.Append(@int.ToString());
            result.Append(".");
            if (eps == 1) result.Append("0");
            frac.Numerator *= 10;
            for (int i = 0; i < eps - 1; i++)
            {
                if (frac.Numerator < frac.Denominator)
                {
                    frac.Numerator *= 10;
                    result.Append("0");
                }
                temp = (int)frac.Numerator / (int)frac.Denominator;
                result.Append(temp.ToString());
                frac.Numerator -= temp * frac.Denominator;
                frac.Numerator *= 10;
            }
            return result;
        }


        public static Fraction Mult(Fraction num1, Fraction num2)
        {
            num1.Numerator *= num2.Numerator;
            num1.Denominator *= num2.Denominator;
            num1.Reduction();
            return num1;
        }
        public static Fraction Div(Fraction num1, Fraction num2)
        {
            if (num2.Numerator == 0)
                throw new DivideByZeroException("NO GOD! PLEASE NOOOOO!!!");
            num1.Numerator *= num2.Denominator;
            num1.Denominator *= num2.Numerator;
            num1.Reduction();
            return num1;
        }
        public static Fraction Add(Fraction num1, Fraction num2)
        {
            num1.Numerator *= num2.Denominator;
            num2.Numerator *= num1.Denominator;
            num1.Denominator *= num2.Denominator;
            num2.Denominator = num1.Denominator;

            num1.Numerator += num2.Numerator;

            num1.Reduction();

            return num1;
        }
        public static Fraction Sub(Fraction num1, Fraction num2)
        {
            num1.Numerator *= num2.Denominator;
            num2.Numerator *= num1.Denominator;
            num1.Denominator *= num2.Denominator;
            num2.Denominator = num1.Denominator;

            num1.Numerator -= num2.Numerator;

            num1.Reduction();

            return num1;
        }

        public static Fraction operator *(Fraction first, Fraction second)
        {
            return Mult(first, second);
        }
        public static Fraction operator /(Fraction first, Fraction second)
        {
            return Div(first, second);
        }
        public static Fraction operator +(Fraction first, Fraction second)
        {
            return Add(first, second);
        }
        public static Fraction operator -(Fraction first, Fraction second)
        {
            return Sub(first, second);
        }

        #endregion

        #region Logic
        public static bool operator ==(Fraction left, Fraction right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Fraction left, Fraction right)
        {
            return !left.Equals(right);
        }
        public static bool operator >(Fraction left, Fraction right)
        {
            return left.CompareTo(right) > 0;
        }
        public static bool operator <(Fraction left, Fraction right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator >=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) >= 0;
        }
        public static bool operator <=(Fraction left, Fraction right)
        {
            return left.CompareTo(right) <= 0;
        }
        #endregion



        public override string ToString()
        {
            return $"<{Numerator}>/<{Denominator}>";
        }
    }
}
