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
        /// 直接下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            using (HttpClient http = new HttpClient())
            {
                var httpResponseMessage = await http.GetAsync("http://localhost:813/新建文件夹2.rar", HttpCompletionOption.ResponseHeadersRead);//发送请求
                var contentLength = httpResponseMessage.Content.Headers.ContentLength;//读取文件大小
                using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())//读取文件流
                {
                    var readLength = 1024000;//1000K  每次读取大小
                    byte[] bytes = new byte[readLength];
                    int writeLength;
                    while ((writeLength = stream.Read(bytes, 0, readLength)) > 0)//分块读取文件流
                    {
                        using (FileStream fs = new FileStream(Application.StartupPath + "/temp.rar", FileMode.Append, FileAccess.Write))//使用追加方式打开一个文件流
                        {
                            fs.Write(bytes, 0, writeLength);//追加写入文件
                            contentLength -= writeLength;
                            if (contentLength == 0)//如果写入完成 给出提示
                                MessageBox.Show("下载完成");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 异步下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button2_ClickAsync(object sender, EventArgs e)
        {
            //开启一个异步线程
            await Task.Run(async () =>
            {
                //异步操作UI元素
                label1.Invoke((Action)(() =>
                {
                    label1.Text = "准备下载...";
                }));

                long downloadSize = 0;//已经下载大小
                long downloadSpeed = 0;//下载速度
                using (HttpClient http = new HttpClient())
                {
                    var httpResponseMessage = await http.GetAsync("http://localhost:813/新建文件夹2.rar", HttpCompletionOption.ResponseHeadersRead);//发送请求
                    var contentLength = httpResponseMessage.Content.Headers.ContentLength;   //文件大小                
                    using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                    {
                        var readLength = 1024000;//1000K
                        byte[] bytes = new byte[readLength];
                        int writeLength;
                        var beginSecond = DateTime.Now.Second;//当前时间秒
                        while ((writeLength = stream.Read(bytes, 0, readLength)) > 0)
                        {
                            //使用追加方式打开一个文件流
                            using (FileStream fs = new FileStream(Application.StartupPath + "/temp.rar", FileMode.Append, FileAccess.Write))
                            {
                                fs.Write(bytes, 0, writeLength);
                            }
                            downloadSize += writeLength;
                            downloadSpeed += writeLength;
                            progressBar1.Invoke((Action)(() =>
                            {
                                var endSecond = DateTime.Now.Second;
                                if (beginSecond != endSecond)//计算速度
                                {
                                    downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                                    label1.Text = "下载速度" + downloadSpeed / 1024 + "KB/S";

                                    beginSecond = DateTime.Now.Second;
                                    downloadSpeed = 0;//清空
                                }
                                progressBar1.Value = Math.Max((int)(downloadSize * 100 / contentLength), 1);
                            }));
                        }

                        label1.Invoke((Action)(() =>
                        {
                            label1.Text = "下载完成";
                        }));
                    }
                }
            });
        }


        /// <summary>
        /// 是否暂停
        /// </summary>
        static bool isPause = true;
        /// <summary>
        /// 下载开始位置（也就是已经下载了的位置）
        /// </summary>
        static long rangeBegin = 0;//(当然，这个值也可以存为持久化。如文本、数据库等)

        /// <summary>
        /// 断线续传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button3_ClickAsync(object sender, EventArgs e)
        {
            isPause = !isPause;
            if (!isPause)//点击下载
            {
                button3.Text = "暂停";

                await Task.Run(async () =>
                {
                    //异步操作UI元素
                    label1.Invoke((Action)(() =>
                   {
                       label1.Text = "准备下载...";
                   }));

                    long downloadSpeed = 0;//下载速度
                    using (HttpClient http = new HttpClient())
                    {
                        //var url = "http://localhost:813/新建文件夹2.rar";  //a标签下载链接
                        var url = "http://localhost:813/FileDownload/FileDownload5";    //我们自己实现的服务端下载链接
                        var request = new HttpRequestMessage { RequestUri = new Uri(url) };
                        request.Headers.Range = new RangeHeaderValue(rangeBegin, null);//【关键点】全局变量记录已经下载了多少，然后下次从这个位置开始下载。
                        var httpResponseMessage = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                        var contentLength = httpResponseMessage.Content.Headers.ContentLength;//本次请求的内容大小
                        if (httpResponseMessage.Content.Headers.ContentRange != null) //如果为空，则说明服务器不支持断点续传
                        {
                            contentLength = httpResponseMessage.Content.Headers.ContentRange.Length;//服务器上的文件大小
                        }

                        using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                        {
                            var readLength = 1024000;//1000K
                            byte[] bytes = new byte[readLength];
                            int writeLength;
                            var beginSecond = DateTime.Now.Second;//当前时间秒
                            while ((writeLength = stream.Read(bytes, 0, readLength)) > 0 && !isPause)
                            {
                                //使用追加方式打开一个文件流
                                using (FileStream fs = new FileStream(Application.StartupPath + "/temp.rar", FileMode.Append, FileAccess.Write))
                                {
                                    fs.Write(bytes, 0, writeLength);
                                }
                                downloadSpeed += writeLength;
                                rangeBegin += writeLength;
                                progressBar1.Invoke((Action)(() =>
                                {
                                    var endSecond = DateTime.Now.Second;
                                    if (beginSecond != endSecond)//计算速度
                                    {
                                        downloadSpeed = downloadSpeed / (endSecond - beginSecond);
                                        label1.Text = "下载速度" + downloadSpeed / 1024 + "KB/S";

                                        beginSecond = DateTime.Now.Second;
                                        downloadSpeed = 0;//清空
                                    }
                                    progressBar1.Value = Math.Max((int)((rangeBegin) * 100 / contentLength), 1);
                                }));
                            }

                            if (rangeBegin == contentLength)
                            {
                                label1.Invoke((Action)(() =>
                                {
                                    label1.Text = "下载完成";
                                }));
                            }
                        }
                    }
                });
            }
            else//点击暂停
            {
                button3.Text = "继续下载";
                label1.Text = "暂停下载";
            }
        }

        /// <summary>
        /// 多线程下载文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void button4_ClickAsync(object sender, EventArgs e)
        {
            using (HttpClient http = new HttpClient())
            {
                var url = "http://localhost:813/FileDownload/FileDownload5";
                var httpResponseMessage = await http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                var contentLength = httpResponseMessage.Content.Headers.ContentLength.Value;
                var size = contentLength / 10; //这里为了方便，就直接分成10个线程下载。（当然这是不合理的）
                var tasks = new List<Task>();
                for (int i = 0; i < 10; i++)
                {
                    var begin = i * size;
                    var end = begin + size - 1;
                    var task = FileDownload(url, begin, end, i);
                    tasks.Add(task);
                }
                for (int i = 0; i < 10; i++)
                {
                    await tasks[i];  //当然，这里如有下载异常没有考虑、文件也没有校验。各位自己完善吧。
                    progressBar1.Value = (i + 1) * 10;
                }
                FileMerge(Application.StartupPath + @"\File", "temp.rar");
                label1.Text = "下载完成";
            }
        }

        /// <summary>
        /// 文件下载
        /// （如果你有兴趣，可以没个线程弄个进度条）
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Task FileDownload(string url, long begin, long end, int index)
        {
            var task = Task.Run(async () =>
            {
                using (HttpClient http = new HttpClient())
                {
                    var request = new HttpRequestMessage { RequestUri = new Uri(url) };
                    request.Headers.Range = new RangeHeaderValue(begin, end);//【关键点】全局变量记录已经下载了多少，然后下次从这个位置开始下载。
                    var httpResponseMessage = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                    using (var stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                    {
                        var readLength = 1024000;//1000K
                        byte[] bytes = new byte[readLength];
                        int writeLength;
                        var beginSecond = DateTime.Now.Second;//当前时间秒
                        var filePaht = Application.StartupPath + "/File/";
                        if (!Directory.Exists(filePaht))
                            Directory.CreateDirectory(filePaht);

                        try
                        {
                            while ((writeLength = stream.Read(bytes, 0, readLength)) > 0)
                            {
                                //使用追加方式打开一个文件流
                                using (FileStream fs = new FileStream(filePaht + index, FileMode.Append, FileAccess.Write))
                                {
                                    fs.Write(bytes, 0, writeLength);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //如果出现异常则删掉这个文件
                            File.Delete(filePaht + index);
                        }
                    }
                }
            });

            return task;
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileMerge(string path, string fileName)
        {
            //这里排序一定要正确，转成数字后排序（字符串会按1 10 11排序，默认10比2小）
            foreach (var filePath in Directory.GetFiles(path).OrderBy(t => int.Parse(Path.GetFileNameWithoutExtension(t))))
            {
                using (FileStream fs = new FileStream(Directory.GetParent(path).FullName + @"\" + fileName, FileMode.Append, FileAccess.Write))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes(filePath);//读取文件到字节数组
                    fs.Write(bytes, 0, bytes.Length);//写入文件
                }
                System.IO.File.Delete(filePath);
            }
            Directory.Delete(path);
            return true;
        }
    }
}
