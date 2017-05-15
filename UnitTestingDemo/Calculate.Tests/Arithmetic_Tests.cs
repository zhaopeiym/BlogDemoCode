//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Calculate.Tests
{
    public class Arithmetic_Tests
    {
        [Fact]
        public void Add_Ok()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(2, 3);
            Assert.Equal(5, sum);
        }

        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(2, 4, 6)]
        [InlineData(8, 3, 11)]
        public void Add_Theory_Ok(int a, int b, int c)
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(a, b);
            Assert.True(c == sum);
            //Assert.Equal(c, sum);
        }

        [Fact]
        public void Add_No()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Add(2, 3);
            Assert.NotEqual(6, sum);
        }

        [Fact]
        public void Subtract_Ok()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Subtract(2, 3);
            Assert.Equal(-1, sum);

            //Assert.True(-1 == sum);
        }

        [Fact]
        public void Subtract_No()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Subtract(2, 3);
            Assert.NotEqual(1, sum);
        }

        [Fact]
        public void Compare_Ok()
        {
            Arithmetic arithmetic = new Arithmetic();
            var isGreater = arithmetic.Compare(3, 2);
            Assert.True(isGreater);
        }

        [Fact]
        public void Divide_Ok()
        {
            var sub = Substitute.For<Arithmetic>();
            //sub.Compare(4, 2).Returns(true);
            sub.Compare(Arg.Any<int>(), Arg.Any<int>()).Returns(true);
            var returnNumber = sub.Divide(4, 2);
            Assert.True(returnNumber == 2); 

            Assert.True(sub.Divide(4, 2) == 4 / 2);
            Assert.True(sub.Divide(1, 2) == 0);

            sub.Compare(Arg.Any<int>(), Arg.Any<int>()).Returns(false);
            Assert.True(sub.Divide(4, 2) == -1);
            Assert.True(sub.Divide(1, 2) == -1);

            Arithmetic arithmetic = new Arithmetic();
            //var obj = new Microsoft.VisualStudio.TestTools.UnitTesting.PrivateObject(arithmetic);
            //Assert.True((bool)obj.Invoke("PrivateCompare", 3, 2));
            //Assert.False((bool)obj.Invoke("PrivateCompare", 1, 2));            
        }

    }

}
