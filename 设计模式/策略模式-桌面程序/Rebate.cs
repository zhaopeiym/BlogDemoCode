using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式_桌面程序
{
    //折扣计算
    public class Rebate : AmountCalculation
    {
        private double discountRate;
        public Rebate(double discountRate)
        {
            this.discountRate = discountRate;
        }

        public override double Calculation(double price, double number)
        {
            return price * number * discountRate;
        }
    }
}
