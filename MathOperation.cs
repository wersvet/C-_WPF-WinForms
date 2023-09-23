using System;

namespace _2_lesson
{
    public static class MathOperation
    {
        public static string Add(string num1, string num2)
        {
            double a;
            double b;

            if (!Double.TryParse(num1, out a) || !Double.TryParse(num2, out b)) { return null; }
            return (a + b).ToString();
        }

        public static string Sub(string num1, string num2)
        {
            double a;
            double b;

            if (!Double.TryParse(num1, out a) || !Double.TryParse(num2, out b)) { return null; }
            return (a - b).ToString();
        }

        public static string Mul(string num1, string num2)
        {
            double a;
            double b;

            if (!Double.TryParse(num1, out a) || !Double.TryParse(num2, out b)) { return null; }
            return (a * b).ToString();
        }

        public static string Dev(string num1, string num2)
        {
            double a;
            double b;

            if (!Double.TryParse(num1, out a) || !Double.TryParse(num2, out b)) { return null; }
            return (a / b).ToString();
        }

        public static string Proc(string num1, string num2)
        {
            double a;
            double b;

            if (!Double.TryParse(num1, out a) || !Double.TryParse(num2, out b)) { return null; }
            return (a * b / 100).ToString();
        }

        public static string Sqr(string num1)
        {
            double a;

            if (!Double.TryParse(num1, out a)) { return null; }
            return Math.Sqrt(a).ToString();
        }

        public static string Pow(string num1)
        {
            double a;

            if (!Double.TryParse(num1, out a)) { return null; }
            return Math.Pow(a, 2).ToString();
        }

        public static string OneDev(string num1)
        {
            double a;

            if (!Double.TryParse(num1, out a)) { return null; }
            return (1 / a).ToString();
        }
    }
}