using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    
    public class OrderManage
    {
        IEmailHelper email;
        public OrderManage(IEmailHelper email)
        {
            this.email = email;
        }

        public bool ModifyOrder()
        {
            //.....
          
           return  email.Send();

            //return true;
        }
    }
}
