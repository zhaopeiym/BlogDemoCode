using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 窗体程序
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "...";
            var request = WebRequest.Create("https://github.com/");
            request.BeginGetResponse(new AsyncCallback(t =>
            {
                //（1）处理请求结果的逻辑必须写这里
                label1.Invoke((Action)(() => { label1.Text = "[旧异步]执行完毕！"; }));//（2）这里跨线程访问UI需要做处理             
            }), null);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "...";
            HttpClient http = new HttpClient();
            var htmlStr = await http.GetStringAsync("https://github.com/");
            //（1）处理请求结果的逻辑可以写这里
            label1.Text = "[新异步]执行完毕！";//（2）不在需要做跨线程UI处理了
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.Text = "...";
            HttpClient http = new HttpClient();
            var htmlStr = http.GetStringAsync("https://github.com/").Result;
            //var htmlStr = http.GetStringAsync("https://github.com/").GetAwaiter().GetResult();            
            //（1）处理请求结果的逻辑可以写这里
            label1.Text = "[同步]执行完毕！";//（2）不在需要做跨线程UI处理了
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Text = GetUlrString("https://github.com/").Result;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            label1.Text = AsyncHelper.RunSync(() => GetUlrString("https://github.com/"));
        }

        public async Task<string> GetUlrString(string url)
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetStringAsync(url);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label1.Text = "";
            Task.Run(() =>
            {
                TestResultUrl("http://localhost:803/api/Home?str=同步处理");
            });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label1.Text = "";
            Task.Run(() =>
            {
                TestResultUrl("http://localhost:803/api/Home?str=异步处理");
            });
        }

        public void TestResultUrl(string url)
        {
            int resultEnd = 0;
            HttpClient http = new HttpClient();

            int number = 10;
            for (int i = 0; i < number; i++)
            {
                new Thread(async () =>
                {
                    var resultStr = await http.GetStringAsync(url);
                    label1.Invoke((Action)(() =>
                    {
                        textBox1.AppendText(resultStr.Replace(" ", "\r\t") + "\r\n");
                        if (++resultEnd >= number)
                        {
                            label1.Text = "全部执行完毕";
                        }
                    }));

                }).Start();
            }
        }

        private async void button8_Click(object sender, EventArgs e)
        {          

            #region 方式一
            //try
            //{
            //    Task<string> task = GetUlrStringCheck(null);
            //    Thread.Sleep(1000);//一段逻辑。。。。
            //    textBox1.Text = await task;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}    
            #endregion

            #region 方式二
            try
            {
                Task<string> task = GetUlrStringErr(null);
                Thread.Sleep(1000);//一段逻辑。。。。
                textBox1.Text = await task;
            }
            catch (Exception)
            {
                throw;
            }
            #endregion
        }

        public Task<string> GetUlrStringErr(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("url不能为空");
            }
            Func<Task<string>> func = async () =>
            {
                using (HttpClient http = new HttpClient())
                {
                    return await http.GetStringAsync(url);
                }
            };
            return func();
        }



        public Task<string> GetUlrStringCheck(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("url不能为空");
            }
            return GetUlrStringInfo(url);
        }

        public async Task<string> GetUlrStringInfo(string url)
        {
            using (HttpClient http = new HttpClient())
            {
                return await http.GetStringAsync(url);
            }
        }
    }
}
