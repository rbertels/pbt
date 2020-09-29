using System;
using Xunit;
using FsCheck;
using FsCheck.Xunit;

namespace pbt
{
    public class Tests
    {
        [Property]
        public void TestPositive(Int32 input)
        {
            Assert.True(input >= Int32.MinValue);
        }
    }
}
