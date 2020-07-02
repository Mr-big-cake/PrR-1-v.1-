using System;

namespace PrR_1_v._1_
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction instance1 = new Fraction (49, 100);
            Fraction instance2 = new Fraction(5, 677);
            Fraction instance3 = new Fraction(1000, 10);
            Console.WriteLine(Fraction.Decimal(instance2, 1));
            Console.WriteLine(Fraction.Sqrt(instance1));

        }
    }
}
