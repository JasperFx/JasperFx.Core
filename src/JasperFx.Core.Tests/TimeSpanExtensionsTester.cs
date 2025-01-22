using Shouldly;

namespace JasperFx.Core.Tests
{
    
    public class TimeSpanExtensionsTester
    {
        [Fact]
        public void to_time_from_int()
        {
            700.ToTime().ShouldBe(new TimeSpan(7, 0, 0));
            1700.ToTime().ShouldBe(new TimeSpan(17, 0, 0));
            1850.ToTime().ShouldBe(new TimeSpan(18, 50, 0));
        }

        [Fact]
        public void to_time_from_string()
        {
            "0700".ToTime().ShouldBe(new TimeSpan(7, 0, 0));
            "1700".ToTime().ShouldBe(new TimeSpan(17, 0, 0));
            "1850".ToTime().ShouldBe(new TimeSpan(18, 50, 0));
        }

        [Fact]
        public void Minutes()
        {
            5.Minutes().ShouldBe(new TimeSpan(0, 5, 0));
        }

        [Fact]
        public void hours()
        {
            6.Hours().ShouldBe(new TimeSpan(6, 0, 0));
        }

        [Fact]
        public void days()
        {
            2.Days().ShouldBe(new TimeSpan(2, 0, 0, 0));
        }

        [Fact]
        public void seconds()
        {
            8.Seconds().ShouldBe(new TimeSpan(0, 0, 8));
        }

        [Fact]
        public void to_display()
        {
            4.Seconds().ToDisplay().ShouldBe("4 seconds");
            1.Seconds().ToDisplay().ShouldBe("1 second");
            2.Hours().ToDisplay().ShouldBe("2 hours");
            250.Milliseconds().ToDisplay().ShouldBe("250 milliseconds");
            new TimeSpan(1, 2, 3, 4, 5).ToDisplay()
                .ShouldBe("1 day, 2 hours, 3 minutes, 4 seconds, 5 milliseconds");
        }
    }
}