using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo
{
    #region DB
    public class DB : DbContext
    {
        public DB() : base("Default")
        { }

        public DB(string db)
            : base(db)
        {

        }
        public DbSet<Student> Students { get; set; }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    #endregion

    public class StudentRepositories
    {
        DB db = null;
        public StudentRepositories()
        {
            db = new DB();
        }

        public void Add(Student model)
        {
            db.Set<Student>().Add(model);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = Students.Where(t => t.Id == id).Single();
            db.Set<Student>().Attach(model);
            db.Set<Student>().Remove(model);
            db.SaveChanges();
        }

        public IQueryable<Student> Students
        {
            get
            {
                return db.Students;
            }
        }
    }

    public class StudentService
    {
        StudentRepositories studentRepositories;
        Notiy notiy;
        public StudentService(StudentRepositories studentRepositories, Notiy notiy)
        {
            this.studentRepositories = studentRepositories;
            this.notiy = notiy;
        }

        public bool Create(Student student)
        {
            studentRepositories.Add(student);
            notiy.Info("新来了一个同学" + student.Name);

            //其他一些逻辑
            return true;
        }
    }

    public class Notiy
    {
        public string messg { get; set; }
        public void Info(string messg)
        {
            //发送消息
            this.messg = messg;
        }
    }
}
