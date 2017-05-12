using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Calculate.Tests
{
    public class OrderManage_Tests
    {
        [Fact]
        public void ModifyOrder_Ok()
        {
            IEmailHelper emailHelper = new MyEmailHelper();
            OrderManage manage = new OrderManage(emailHelper);
            var isModeifyOk= manage.ModifyOrder();
            Assert.Equal(true, isModeifyOk);
        }

        public class MyEmailHelper : IEmailHelper
        {
            public bool Send()
            {
                return true;
            }
        }

        [Fact]
        public void ModifyOrder_Err()
        {
            IEmailHelper emailHelper = new EmailHelper();
            OrderManage manage = new OrderManage(emailHelper);            
            Assert.Throws<Exception>(()=> { manage.ModifyOrder(); });// (true, isModeifyOk);
        }

        //.................

        [Fact]
        public void ModifyOrder_NSubstitute_Ok()
        {
            IEmailHelper emailHelper = Substitute.For<IEmailHelper>();
            emailHelper.Send().Returns(true);
            OrderManage manage = new OrderManage(emailHelper);
            var isModeifyOk = manage.ModifyOrder();
            Assert.Equal(true, isModeifyOk);
        } 

    }
}
