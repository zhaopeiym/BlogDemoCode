using System;
using Xunit;

namespace TestDemo.Tests
{
    public class Arithmetic_Tests
    {
        [Fact]
        public void Add_Ok()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(1, 2);
            Assert.True(sum == 3);
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(2, 4, 6)]
        [InlineData(2, 1, 3)]
        public void Add_Ok_Two(int nb1, int nb2, int result)
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(nb1, nb2);
            Assert.True(sum == result);
        }

        [Theory]
        [InlineData(2, 3, 0)]
        [InlineData(2, 4, 0)]
        [InlineData(2, 1, 0)]         
        public void Add_No(int nb1, int nb2, int result)
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(nb1, nb2);
            Assert.False(sum == result);
        }

        [Fact]      
        public void Divide_Err()
        {
            Arithmetic arithmetic = new Arithmetic(); 
            Assert.Throws<Exception>(() => { arithmetic.Divide(4, 0); });//断言 验证异常
        }


    }
}
