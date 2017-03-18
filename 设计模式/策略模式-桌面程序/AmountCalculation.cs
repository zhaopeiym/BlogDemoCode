using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式_桌面程序
{
    public abstract class AmountCalculation
    {
        public abstract double Calculation(double price, double number);
    }
}
