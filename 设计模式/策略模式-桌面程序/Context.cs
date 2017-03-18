using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式_桌面程序
{
    public class Context
    {
        //策略抽象类
        private AmountCalculation amountCalculation;

        public Context(string type)
        {
            switch (type)
            {
                //新增
                case "7.8折":
                    amountCalculation = new Rebate(0.78);
                    break;
                case "8.8折":
                    amountCalculation = new Rebate(0.88);
                    break;
                case "9.8折":
                    amountCalculation = new Rebate(0.98);
                    break;
                case "满300返40":
                    amountCalculation = new Cashback(300, 40);
                    break;
                case "满600返100":
                    amountCalculation = new Cashback(600, 100);
                    break;
               
            }
        }
        //计算金额
        public double Calculation(double price, double number)
        {
            return amountCalculation.Calculation(price, number);
        }
    }
}
