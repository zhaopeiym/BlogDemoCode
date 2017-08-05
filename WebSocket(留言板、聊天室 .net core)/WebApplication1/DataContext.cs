using Microsoft.EntityFrameworkCore;
using MySQL.Data.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    /// <summary>
    /// 留言内容
    /// </summary>
    public class Message
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }
        [MaxLength(20)]
        public string IP { get; set; }
        public DateTime CreateTime { get; set; }

        [Required]
        [MaxLength(80)]
        public string Content { get; set; }
    }

    /// <summary>
    /// 聊天内容
    /// </summary>
    public class ChatInfo
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }
        [MaxLength(20)]
        public string IP { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [MaxLength(80)]
        public string Content { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }
    }

    /// <summary>
    /// 日志，主要记录日志
    /// </summary>
    public class LogInfo
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string IP { get; set; }
        public string Content { get; set; }
    }

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        //这里也可以
        //string str = @"Data Source=192.168.2.145;Database=testMySql;User ID=root;Password=password;pooling=true;CharSet=utf8;port=3306;sslmode=none";       
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        //    optionsBuilder.UseMySQL(str);

        public DbSet<Message> Messages { get; set; }

        public DbSet<LogInfo> LogInfos { get; set; }

        public DbSet<ChatInfo> ChatInfo { get; set; }
    }
}
