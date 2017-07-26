using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace 上传下载.Controllers
{
    public class WebuploaderController : Controller
    {
        // GET: Webuploader
        public ActionResult Index()
        {
            return View();
        }

        public string SaveFile()
        {
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(Server.MapPath("~/App_Data/") + Path.GetFileName(Request.Files[0].FileName));
                return "保存成功";
            }
            return "没有读到文件";
        }

        public string SveFile2()
        {
            //保存文件到根目录 App_Data + 获取文件名称和格式
            var filePath = Server.MapPath("~/App_Data/") + Path.GetFileName(Request.Files[0].FileName);
            //创建一个追加（FileMode.Append）方式的文件流
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    //读取文件流
                    BinaryReader br = new BinaryReader(Request.Files[0].InputStream);
                    //将文件留转成字节数组
                    byte[] bytes = br.ReadBytes((int)Request.Files[0].InputStream.Length);
                    //将字节数组追加到文件
                    bw.Write(bytes);
                }
            }
            return "保存成功";
        }

        public string SveFile3()
        {
            var chunk = Request.Form["chunk"];//当前是第多少片 

            var path = Server.MapPath("~/App_Data/") + Path.GetFileNameWithoutExtension(Request.Files[0].FileName);
            if (!Directory.Exists(path))//判断是否存在此路径，如果不存在则创建
            {
                Directory.CreateDirectory(path);
            }
            //保存文件到根目录 App_Data + 获取文件名称和格式
            var filePath = path + "/" + chunk;
            //创建一个追加（FileMode.Append）方式的文件流
            using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    //读取文件流
                    BinaryReader br = new BinaryReader(Request.Files[0].InputStream);
                    //将文件留转成字节数组
                    byte[] bytes = br.ReadBytes((int)Request.Files[0].InputStream.Length);
                    //将字节数组追加到文件
                    bw.Write(bytes);
                }
            }           
            return "保存成功";
        }

        /// <summary>
        /// 合并文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool FileMerge()
        {
            var fileName = Request.Form["fileName"];
            var path = Server.MapPath("~/App_Data/") + Path.GetFileNameWithoutExtension(fileName);

            //这里排序一定要正确，转成数字后排序（字符串会按1 10 11排序，默认10比2小）
            foreach (var filePath in Directory.GetFiles(path).OrderBy(t => int.Parse(Path.GetFileNameWithoutExtension(t))))
            {
                using (FileStream fs = new FileStream(Server.MapPath("~/App_Data/") + fileName, FileMode.Append, FileAccess.Write))
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