using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式_桌面程序
{
    // 返现
    public class Cashback : AmountCalculation
    {
        //满多少
        private double exceed;
        //返多少
        private double retreat;
        public Cashback(double exceed, double retreat)
        {
            this.exceed = exceed;
            this.retreat = retreat;
        }
        public override double Calculation(double price, double number)
        {
            var momoney = price * number;
            if (momoney >= exceed)
            {
                return momoney - retreat;
            }
            return momoney;
        }
    }
}
