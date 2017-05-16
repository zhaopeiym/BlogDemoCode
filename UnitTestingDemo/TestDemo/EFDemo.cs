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

    public interface IStudentRepositories
    {
        void Add(Student model);
    }

    public class StudentRepositories : IStudentRepositories
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
        IStudentRepositories studentRepositories;
        Notiy notiy;
        public StudentService()
        {
        }
        public StudentService(IStudentRepositories studentRepositories)
        {
            this.studentRepositories = studentRepositories;
        }
        public StudentService(IStudentRepositories studentRepositories, Notiy notiy)
        {
            this.studentRepositories = studentRepositories;
            this.notiy = notiy;
        }

        public bool Create(Student student)
        {
            studentRepositories.Add(student);

            return true;
        }

        public bool CreateAndNotiy(Student student)
        {
            studentRepositories.Add(student);
            bool isNotiyOk = false;
            try
            {
                isNotiyOk = notiy.Info("" + student.Name);//消息通知
            }
            catch (Exception)
            {
                //记录日志等
                //.......
            }
            //其他一些逻辑
            return isNotiyOk;
        }

        private bool XXXInit()
        {
            return true;
        }
    }

    public class Notiy
    {
        public virtual bool Info(string messg)
        {
            //发送消息、邮件发送、短信发送。。。
            //.........
            if (string.IsNullOrWhiteSpace(messg))
            {
                return false;
            }
            return true;
        }
    }
}
