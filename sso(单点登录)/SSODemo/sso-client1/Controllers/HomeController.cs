using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace sso_client1.Controllers
{
    public class HomeController : Controller
    {
        public static List<string> Tokens = new List<string>();
        public async Task<ActionResult> Index()
        {
            var tokenId = Request.QueryString["tokenId"];
            if (tokenId != null)
            {
                using (HttpClient http = new HttpClient())
                {
                    //验证Tokend是否有效
                    var isValid = await http.GetStringAsync("http://localhost:810/Home/TokenIdIsValid?tokenId=" + tokenId);
                    if (bool.Parse(isValid.ToString()))
                    {
                        if (!Tokens.Contains(tokenId))
                        {
                            Tokens.Add(tokenId);
                        }
                        Session["token"] = tokenId;
                    }
                }
            }
            if (Session["token"] == null || !Tokens.Contains(Session["token"].ToString()))
            {
                return Redirect("http://localhost:810/Home/Verification?backUrl=http://localhost:811/Home");
            }
            return View();
        }

        public void ClearToken(string tokenId)
        {
            Tokens.Remove(tokenId);
        }
    }
}