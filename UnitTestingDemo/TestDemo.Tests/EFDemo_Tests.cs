using System;
using System.Linq;
using Xunit;

namespace TestDemo.Tests
{
    public class StudentRepositories_Tests: IDisposable
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
}
