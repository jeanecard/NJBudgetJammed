using NJBudgetWBackend.Business;
using System;
using Xunit;

namespace NJBudgetWBackEndTests
{
    public class PeriodProcessorTest
    {
        [Fact]
        public void PeriodProcessor_With_2_monthes_to_5_12_2021_Expect_1_10_2021()
        {
            PeriodProcessor process = new ();
            process.ProcessRangeBeforeTo(new DateTime(2021, 12, 5), 2, out DateTime retour);
            Assert.True(retour.Year == 2021);
            Assert.True(retour.Month == 10);
            Assert.True(retour.Day == 1);
        }

        [Fact]
        public void PeriodProcessor_With_0_month_to_15_6_2021_Expect_1_6_2021()
        {
            PeriodProcessor process = new ();
            process.ProcessRangeBeforeTo(new DateTime(2021, 6, 15), 0, out DateTime retour);
            Assert.True(retour.Year == 2021);
            Assert.True(retour.Month == 6);
            Assert.True(retour.Day == 1);
        }

        [Fact]
        public void PeriodProcessor_With_9_month_to_15_6_2021_Expect_1_9_2020()
        {
            PeriodProcessor process = new ();
            process.ProcessRangeBeforeTo(new DateTime(2021, 6, 15), 9, out DateTime retour);
            Assert.True(retour.Year == 2020);
            Assert.True(retour.Month == 9);
            Assert.True(retour.Day == 1);
        }
    }
}
