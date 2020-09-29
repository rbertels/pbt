using System;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace pbt
{
    public struct TimeSeriesEntries
    {
        public decimal Minimum { get; set; }

        public decimal Maximum { get; set; }

        public int Amount { get; set; }

        public decimal Sum { get; set; } 

        public static TimeSeriesEntries FromMeasurement(decimal value)
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
                Maximum = decimal.MinValue,
                Minimum = decimal.MaxValue,
                Sum = 0
            };
        }
    }


    public class TimeSeriesTests
    {
        class TemperatureMeasurementGenerator
        {
            // A custom generator to make sure we dont get rounding errors
            public static Arbitrary<decimal> Decimal()
            {
                return Arb.Default.Int32().Generator.Select(x => (decimal)x / 100.0m).ToArbitrary();
            }
        }

        [Property(Arbitrary = new[] { typeof(TemperatureMeasurementGenerator) })]
        public void TestCombineWithIdentityChangesNothing(decimal value)
        {
            var ts = TimeSeriesEntries.FromMeasurement(value);

            var r1 = TimeSeriesEntries.Combine(ts, TimeSeriesEntries.Identity());
            var r2 = TimeSeriesEntries.Combine(TimeSeriesEntries.Identity(), ts);

            Assert.Equal(ts, r1);
            Assert.Equal(ts, r2);
        }


        [Property(Arbitrary = new[] { typeof(TemperatureMeasurementGenerator) })]
        public void TestAssociativity(decimal v1, decimal v2, decimal v3)
        {
            var ts1 = TimeSeriesEntries.FromMeasurement(v1);
            var ts2 = TimeSeriesEntries.FromMeasurement(v2);
            var ts3 = TimeSeriesEntries.FromMeasurement(v3);

            var r1 = TimeSeriesEntries.Combine(ts1, TimeSeriesEntries.Combine(ts2, ts3));
            var r2 = TimeSeriesEntries.Combine(TimeSeriesEntries.Combine(ts1, ts2), ts3);

            Assert.Equal(r1, r2);
        }
    }
}