using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace калькулятор
{
    static internal class MathOperations
    {
        static public bool is_error = false;

        static public string Add(string a, string b)
        {
            if (double.TryParse(a, out double d) & double.TryParse(b, out double c)) return (d + c).ToString();
            else return null; 
            //return a + b;
        }
        static public string Sub(string a, string b)
        {
            if (double.TryParse(a, out double d) & double.TryParse(b, out double c)) return (d - c).ToString();
            else return null;
        }
        static public string Equals(string a)
        {
            if (double.TryParse(a, out double d)) return a;
            else return null;
        }
        static public string Del(string a, string b)
        {
            if (double.TryParse(a, out double d) & double.TryParse(b, out double c) & c != 0) return (d / c).ToString();
            else if (c == 0 & b != null) {
                is_error = true;
                return "Делить на ноль нельзя";
            }
            else return null;
        }
        static public string Mult(string a, string b)
        {
            if (double.TryParse(a, out double d) & double.TryParse(b, out double c)) return (d * c).ToString();
            else return null;
        }
        static public string Sqr(string a)
        {
            if (double.TryParse(a, out double d) & (d >= 0)) return (Math.Sqrt(d)).ToString();
            else if (d < 0)
            {
                is_error = true;
                return "Недопустимый ввод";
            }
            else return null;
        }
        static public string Pow(string a)
        {
            if (double.TryParse(a, out double d)) return (Math.Pow(d, 2)).ToString();
            else return null;
        }
        static public string Fract(string a)
        {
            if (double.TryParse(a, out double d) & d != 0) return (1 / d).ToString();
            else if (d == 0)
            {
                is_error = true;
                return "Делить на ноль нельзя";
            }
            else return null;
        }
        static public string Neg(string a)
        {
            if (double.TryParse(a, out double d)) return (d*(-1)).ToString();
            //else if (d == 0) return "Деление на ноль невозможно";
            else return null;
        }
    }
}
