using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 文件下载_客户端_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            button1.Enabled = false;
            var size = 0;

            await Task.Run(async () =>
            {
                label1.Invoke((Action)(() =>
                {
                    label1.Text = "准备下载...";
                }));
                using (HttpClient http = new HttpClient())
                {
                    var response = await http.GetAsync("http://localhost:813/FileDownload/FileDownload5");
                    var maxLength = response.Content.Headers.ContentLength;

                    float count = 0;
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var readLength = 1024000;//1000K
                        byte[] bytes = new byte[readLength];
                        int writeLength;
                        var second = DateTime.Now.Second;
                        while ((writeLength = stream.Read(bytes, 0, readLength)) > 0)
                        {
                            size += readLength;
                            progressBar1.Invoke((Action)(() =>
                            {
                                if (second != DateTime.Now.Second)
                                {
                                    second = DateTime.Now.Second;
                                    label1.Text = "下载速度" + size / 1024 + "KB/S";
                                    size = 0;
                                }
                                progressBar1.Value = (int)((float)readLength * (++count) * 100 / maxLength);
                                if (progressBar1.Value == 100)
                                {
                                    label1.Text = "下载完成";
                                    button1.Enabled = true;
                                }
                            }));
                            using (FileStream fs = new FileStream(Application.StartupPath + "/temp.rar", FileMode.Append, FileAccess.Write))
                            {
                                fs.Write(bytes, 0, writeLength);
                            }
                        }
                    }
                }
            });
        }


        /// <summary>
        /// 是否暂停
        /// </summary>
        static bool isPause = true;

        static long RangeBegin = 0;

        /// <summary>
        /// 下载/暂停（续传）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            isPause = !isPause;
            if (!isPause)//点击下载
            {
                button2.Text = "暂停";

                var size = 0;
                await Task.Run(async () =>
                {
                    label1.Invoke((Action)(() =>
                    {
                        label1.Text = "准备下载...";
                    }));

                    using (HttpClient http = new HttpClient())
                    {
                        var request = new HttpRequestMessage { RequestUri = new Uri("http://localhost:813/FileDownload/FileDownload5") };
                        request.Headers.Range = new RangeHeaderValue(RangeBegin, null);
                        var response = await http.SendAsync(request);
                        var maxLength = response.Content.Headers.ContentLength;

                        if (response.Content.Headers.ContentRange != null) //如果为空，则说明服务器不支持断点续传
                        {
                            maxLength = response.Content.Headers.ContentRange.Length;
                        }                       

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            var readLength = 1024000;//1000K
                            byte[] bytes = new byte[readLength];
                            int writeLength;

                            //long temp = 0;
                            //while (RangeBegin > temp)
                            //{
                            //    temp += stream.Read(bytes, 0, readLength);
                            //}  

                            var second = DateTime.Now.Second;
                            while ((writeLength = stream.Read(bytes, 0, readLength)) > 0 && !isPause)
                            {                                
                                using (FileStream fs = new FileStream(Application.StartupPath + "/temp.rar", FileMode.Append, FileAccess.Write))
                                {
                                    fs.Write(bytes, 0, writeLength);
                                    RangeBegin += readLength;
                                }
                                size += readLength;
                                progressBar1.Invoke((Action)(() =>
                                {
                                    if (second != DateTime.Now.Second)
                                    {
                                        second = DateTime.Now.Second;
                                        label1.Text = "下载速度" + size / 1024 + "KB/S";
                                        size = 0;
                                    }
                                    progressBar1.Value = Math.Max((int)(RangeBegin * 100 / maxLength), 1);
                                    if (progressBar1.Value == 100)
                                    {
                                        label1.Text = "下载完成";
                                    }
                                }));
                            }

                            label1.Invoke((Action)(() =>
                            {
                                label1.Text = label1.Text == "下载完成" ? "下载完成" : "暂停下载";
                            }));
                        }
                    }
                });
            }
            else//点击暂停
            {
                button2.Text = "继续下载";
            }
        }
    }
}
