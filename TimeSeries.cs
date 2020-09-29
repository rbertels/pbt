using System;
using FsCheck.Xunit;
using Xunit;

namespace pbt
{
    public struct TimeSeriesEntries
    {
        public float Minimum { get; set; }

        public float Maximum { get; set; }

        public int Amount { get; set; }

        public double Sum { get; set; }

        public static TimeSeriesEntries FromMeasurement(float value)
        {
            return new TimeSeriesEntries()
            {
                Amount = 1,
                Minimum = value,
                Maximum = value,
                Sum = value
            };
        }

        public static TimeSeriesEntries Combine(TimeSeriesEntries ts1, TimeSeriesEntries ts2)
        {
            return new TimeSeriesEntries()
            {
                Amount = ts1.Amount + ts2.Amount,
                Maximum = Math.Max(ts1.Maximum, ts2.Maximum),
                Minimum = Math.Min(ts1.Minimum, ts2.Minimum),
                Sum = ts1.Sum + ts2.Sum
            };
        }

        public static TimeSeriesEntries Identity()
        {
            return new TimeSeriesEntries()
            {
                Amount = 0,
                Maximum = float.NegativeInfinity,
                Minimum = float.PositiveInfinity,
                Sum = 0
            };
        }
    }


    public class TimeSeriesTests
    {
        [Property]
        public void TestCombineWithIdentityChangesNothing(float value)
        {
            var ts = TimeSeriesEntries.FromMeasurement(value);
            
            var r1 = TimeSeriesEntries.Combine(ts, TimeSeriesEntries.Identity());
            var r2 = TimeSeriesEntries.Combine(TimeSeriesEntries.Identity(), ts);

            Assert.Equal(ts, r1);
            Assert.Equal(ts, r2);
        }
    }



}