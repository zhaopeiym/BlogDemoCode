using EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLibrary
{
    public class TempDemo
    {
        BloggingContext bloggingContext;
        public TempDemo(BloggingContext bloggingContext)
        {
            this.bloggingContext = bloggingContext;
        }
        //获取DbContext的HashCode
        public int GetDBHashCode()
        {
            return bloggingContext.GetHashCode();
        }
    }
}
