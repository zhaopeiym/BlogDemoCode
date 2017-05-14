using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{

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

        public virtual bool Compare(int a, int b)
        {
            return a - b > 0;
        }

        private bool PrivateCompare(int a, int b)
        {
            return a - b > 0;
        }

        public int Divide(int a, int b)
        {
            if (Compare(a, b))
                return a / b;
            return -1;
        }
    }
}
