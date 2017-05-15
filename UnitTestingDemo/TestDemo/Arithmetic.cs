using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    public class Arithmetic
    {
        public int Add(int nb1, int nb2)
        {
            return nb1 + nb2;
        }

        public int Divide(int nb1, int nb2)
        {
            if (nb2==0)
            {
                throw new Exception("除数不能为零");
            }
            return nb1 / nb2;
        }
    }
}
