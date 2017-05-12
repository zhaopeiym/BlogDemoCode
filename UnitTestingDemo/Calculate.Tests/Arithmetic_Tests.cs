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
            var sum = arithmetic.Add(2,3);
            Assert.Equal(5, sum);
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
        }

        [Fact]
        public void Subtract_No()
        {
            Arithmetic arithmetic = new Arithmetic();
            var sum = arithmetic.Subtract(2, 3);
            Assert.NotEqual(1, sum);
        }

    }
}
