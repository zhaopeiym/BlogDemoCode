using Moq;
using System;
using System.Linq;
using Xunit;

namespace TestDemo.Tests
{
    public class StudentRepositories_Tests : IDisposable
    {
        [Fact]
        public void Add_Ok()
        {
            StudentRepositories r = new StudentRepositories();
            Student student = new Student()
            {
                Id = 1,
                Name = "张三"
            };
            r.Add(student);

            var model = r.Students.Where(t => t.Name == "张三").FirstOrDefault();
            Assert.True(model != null);
        }

        [Fact]
        public void Add_Ok11()
        {
            StudentRepositories r = new StudentRepositories();
            Student student = new Student()
            {
                Id = 1,
                Name = "张三"
            };
            r.Add(student);

            var model = r.Students.Where(t => t.Name == "张三").FirstOrDefault();
            Assert.True(model != null);
        }

        public void Dispose()
        {
            StudentRepositories r = new StudentRepositories();
            var models = r.Students.ToList();
            foreach (var item in models)
            {
                r.Delete(item.Id);
            }
        }
    }

    public class Notiy_Tests
    {
        [Fact]
        public void Info_Ok()
        {
            Notiy notiy = new Notiy();
            var isNotiyOk = notiy.Info("消息发送成功");
            Assert.True(isNotiyOk);
        }

        [Fact]
        public void Info_No()
        {
            Notiy notiy = new Notiy();
            var isNotiyOk = notiy.Info("");
            Assert.False(isNotiyOk);
        }
    }

    public class StudentService_Tests
    {
        [Fact]
        public void Create_Ok()
        {
            IStudentRepositories studentRepositories = new StubStudentRepositories();
            StudentService service = new StudentService(studentRepositories);
            var isCreateOk = service.Create(null);
            Assert.True(isCreateOk);
        }

        public class StubStudentRepositories : IStudentRepositories
        {
            public void Add(Student model)
            {
            }
        }

        [Fact]
        public void Create_Mock_Ok()
        {
            var studentRepositories = new Mock<IStudentRepositories>();
            var notiy = new Mock<Notiy>();
            StudentService service = new StudentService(studentRepositories.Object);
            var isCreateOk = service.Create(null);
            Assert.True(isCreateOk);
        }

        [Fact]
        public void Create_Mock_Notiy_Ok()
        {
            var studentRepositories = new Mock<IStudentRepositories>();
            var notiy = new Mock<Notiy>();
            notiy.Setup(f => f.Info(It.IsAny<string>())).Returns(true);//【1】   
            //notiy.Setup(f => f.Info("")).Returns(false);
            //notiy.Setup(f => f.Info("消息通知")).Returns(true);
            StudentService service = new StudentService(studentRepositories.Object, notiy.Object);
            var isCreateOk = service.CreateAndNotiy(new Student());
            Assert.True(isCreateOk);
        }

        [Fact]
        public void Create_Mock_Notiy_Err()
        {
            var studentRepositories = new Mock<IStudentRepositories>();
            var notiy = new Mock<Notiy>();
            notiy.Setup(f => f.Info(It.IsAny<string>())).Throws(new Exception("异常"));             
            StudentService service = new StudentService(studentRepositories.Object, notiy.Object);
            var isCreateOk = service.CreateAndNotiy(new Student());
            Assert.False(isCreateOk);
        }

        [Fact]
        public void XXXInit_Ok()
        {
            var studentRepositories = new StudentService();
            var obj = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(studentRepositories);
            Assert.True((bool)obj.Invoke("XXXInit"));
        }
    }
}
