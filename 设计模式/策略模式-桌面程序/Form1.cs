using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 策略模式_桌面程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //计算
        private void button1_Click(object sender, EventArgs e)
        {
            #region 重构前
            //var money = price * number;//本次计算金额
            //switch (cmBstrategy.Text)//下拉框
            //{ 
            //    case "8.8折":
            //        money *= 0.88;
            //        break;
            //    case "9.8折":
            //        money *= 0.98;
            //        break;
            //    case "满300返40":
            //        if (money >= 300)
            //        {
            //            money -= 40;
            //        }
            //        break;
            //    case "满600返100":
            //        if (money >= 600)
            //        {
            //            money -= 100;
            //        }
            //        break;
            //} 
            #endregion


            var price = Convert.ToDouble(txtPrice.Text);//单价
            var number = Convert.ToDouble(txtNumber.Text);//数量
            var lastTotal = Convert.ToDouble(labTotal.Text);//已购买金额      
             
            var context = new Context(cmBstrategy.Text);//新增代码
            var money = context.Calculation(price, number);//新增代码

            labTotal.Text = (lastTotal + money).ToString();
            txtContent.Text += string.Format("单价：{0},数量：{1}，促销：{2},实际金额：{3}", price, number, cmBstrategy.Text, money + "\r\n");

            txtPrice.Text = string.Empty;
            txtNumber.Text = string.Empty;
            cmBstrategy.SelectedIndex = -1;
        }

        //重置
        private void button2_Click(object sender, EventArgs e)
        {
            labTotal.Text = "0.00";
            txtPrice.Text = string.Empty;
            txtNumber.Text = string.Empty;
            txtContent.Text = string.Empty;
            cmBstrategy.SelectedIndex = -1;
        }
    }
}
