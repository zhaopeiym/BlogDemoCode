using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    public class EmailInfo
    {
        public string Body;
        public string To;
        public string Subject;

        public EmailInfo(string to, string subject, string body)
        {
            this.To = to;
            this.Subject = subject;
            this.Body = body;
        }
    }

    public interface IEmailHelper
    {
        bool Send();
    }

    public class EmailHelper : IEmailHelper
    {
        EmailInfo info;
        public EmailHelper() { }
        public EmailHelper(string to, string subject, string body)
        {
            info = new EmailInfo(to, subject, body);
        }
        public bool Send()
        {
            if (info == null)
            {
                throw new Exception("EmailInfo不能为空");
            }
            //具体实现
            return true;
        }
    }
}
