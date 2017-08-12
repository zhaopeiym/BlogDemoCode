using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Item
    {
        public int 主键id { get; set; }
        public string 选项描述 { get; set; }
    }

    #region 自定义问卷
    public class 文本类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 单选类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        /// <summary>
        /// 选项集合
        /// </summary>
        public List<Item> Items { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 多选类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        /// <summary>
        /// 选项集合
        /// </summary>
        public List<Item> Items { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 日期类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 打分类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 数字类型题
    {
        public int 主键id { get; set; }
        public int 问卷id { get; set; }
        public string 问题描述 { get; set; }
        //...自定义一些 保存逻辑和校验规则
    }

    public class 自定义问卷
    {
        public int 主键id { get; set; }
        public string 问卷名字 { get; set; }
        public string 问卷描述 { get; set; }

        //...其他字段

        [ForeignKey("问卷id")]
        public virtual List<文本类型题> 文本类型题集合 { get; set; }
        [ForeignKey("问卷id")]
        public virtual List<单选类型题> 单选类型题集合 { get; set; }
        [ForeignKey("问卷id")]
        public virtual List<多选类型题> 多选类型题集合 { get; set; }
        [ForeignKey("问卷id")]
        public virtual List<日期类型题> 日期类型题集合 { get; set; }
        [ForeignKey("问卷id")]
        public virtual List<打分类型题> 打分类型题集合 { get; set; }
        [ForeignKey("问卷id")]
        public virtual List<数字类型题> 数字类型题集合 { get; set; }
    }
    #endregion

    #region 答卷

    public class 文本类型答案
    {
        public int 主键id { get; set; }
        public int 问卷题目id { get; set; }
        public string 答案描述 { get; set; }
    }

    public class 单选类型答案
    {
        public int 主键id { get; set; }
        public int 答卷id { get; set; }
        public int 问卷题目id { get; set; }
        /// <summary>
        /// 选择项ID
        /// </summary>
        public int ItemId { get; set; }
    }

    public class 多选类型答案
    {
        public int 主键id { get; set; }
        public int 答卷id { get; set; }

        public int 问卷题目id { get; set; }
        /// <summary>
        /// 选中项集合
        /// </summary>
        public List<int> ItemIds { get; set; }
    }

    public class 日期类型答案
    {
        public int 主键id { get; set; }
        public int 答卷id { get; set; }
        public int 问卷题目id { get; set; }
        public DateTime 日期 { get; set; }
    }

    public class 打分类型答案
    {
        public int 主键id { get; set; }
        public int 答卷id { get; set; }

        public int 问卷题目id { get; set; }
        public int 分数 { get; set; }
    }

    public class 数字类型答案
    {
        public int 主键id { get; set; }
        public int 答卷id { get; set; }
        public int 问卷题目id { get; set; }
        public int 数字 { get; set; }

    }
    public class 答卷
    {
        public int 主键id { get; set; }
        public string 答题人姓名 { get; set; }
        public string 答题人联系电话 { get; set; }

        //...其他字段

        [ForeignKey("答卷id")]
        public virtual List<文本类型答案> 文本类型答案集合 { get; set; }
        [ForeignKey("答卷id")]
        public virtual List<单选类型答案> 单选类型答案集合 { get; set; }
        [ForeignKey("答卷id")]
        public virtual List<多选类型答案> 多选类型答案集合 { get; set; }
        [ForeignKey("答卷id")]
        public virtual List<日期类型答案> 日期类型答案集合 { get; set; }
        [ForeignKey("答卷id")]
        public virtual List<打分类型答案> 打分类型答案集合 { get; set; }
        [ForeignKey("答卷id")]
        public virtual List<数字类型答案> 数字类型答案集合 { get; set; }
    }
    #endregion
}
