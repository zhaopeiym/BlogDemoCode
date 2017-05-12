using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{

    public interface IArithmetic
    {
        int Add(int a, int b);

        int Subtract(int a, int b);
    }

    public class Arithmetic
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Subtract(int a, int b)
        {
            return a - b;
        }
    }
}
