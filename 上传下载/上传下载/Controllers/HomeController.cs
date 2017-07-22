using System.IO;
using System.Web.Mvc;

namespace 上传下载.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveFile1()
        {
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(Server.MapPath("~/App_Data/") + Path.GetFileName(Request.Files[0].FileName));
                return Content("保存成功");
            }
            return Content("没有读到文件");
        }

        public string SaveFile2()
        {
            if (Request.Files.Count > 0)
            {
                Request.Files[0].SaveAs(Server.MapPath("~/App_Data/") + Path.GetFileName(Request.Files[0].FileName));
                return "保存成功";
            }
            return "没有读到文件";
        }

        public string SaveFile4()
        {
            Request.SaveAs(Server.MapPath("~/App_Data/SaveFile4.data") + "", false);
            return "保存成功";
        }

        public string SaveFile3()
        {
            using (FileStream fs = new FileStream(@"D:\test.data", FileMode.Append, FileAccess.Write))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    BinaryReader br = new BinaryReader(Request.InputStream);
                    bw.Write(br.ReadBytes((int)Request.InputStream.Length));
                }
            }
            return Request.Files.Count.ToString();
        }

        ///// <summary>
        ///// 流转化为字节数组
        ///// </summary>
        ///// <param name="stream">流</param>
        ///// <returns>字节数组</returns>
        //public static byte[] StreamToByte(Stream stream)
        //{            
        //    using (MemoryStream tempStream = new MemoryStream())
        //    {
        //        int bi;
        //        while ((bi = stream.ReadByte()) != -1)
        //        {
        //            tempStream.WriteByte(((byte)bi));
        //        }
        //        return tempStream.ToArray();
        //    };
        //}
    }
}