using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    public class DB : DbContext
    {
        public DB() : base("Default")
        { }
        public DbSet<Student> Students { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
