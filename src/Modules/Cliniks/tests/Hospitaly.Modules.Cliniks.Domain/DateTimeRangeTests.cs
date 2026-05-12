using FluentAssertions;
using Hospitaly.Common.Domain.Common.ValueObjects;

namespace Hospitaly.Modules.Cliniks.Domain;

public class DateTimeRangeTests
{
    private static readonly DateTimeOffset Jan1 = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Jan15 = new(2026, 1, 15, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Feb1 = new(2026, 2, 1, 0, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset Mar1 = new(2026, 3, 1, 0, 0, 0, TimeSpan.Zero);

    public class Create
    {
        [Fact]
        public void Should_Create_Closed_Range_When_End_Is_Provided()
        {
            var result = DateTimeRange.Create(Jan1, Feb1);

            result.IsError.Should().BeFalse();
            result.Value.Start.Should().Be(Jan1);
            result.Value.End.Should().Be(Feb1);
            result.Value.IsClosed.Should().BeTrue();
            result.Value.IsOpenEnded.Should().BeFalse();
        }

        [Fact]
        public void Should_Create_OpenEnded_Range_When_End_Is_Null()
        {
            var result = DateTimeRange.Create(Jan1, null);

            result.IsError.Should().BeFalse();
            result.Value.Start.Should().Be(Jan1);
            result.Value.End.Should().BeNull();
            result.Value.IsOpenEnded.Should().BeTrue();
            result.Value.IsClosed.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_Error_When_End_Is_Before_Start()
        {
            var result = DateTimeRange.Create(Feb1, Jan1);

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("DateTimeRange.Invalid");
            result.FirstError.Description.Should().Be("End date cannot be earlier than start date.");
        }
    }

    public class FromDuration
    {
        [Fact]
        public void Should_Create_Range_When_Duration_Is_Positive()
        {
            var result = DateTimeRange.FromDuration(Jan1, TimeSpan.FromDays(31));

            result.IsError.Should().BeFalse();
            result.Value.Start.Should().Be(Jan1);
            result.Value.End.Should().Be(Feb1);
        }

        [Fact]
        public void Should_Create_Range_When_Duration_Is_Zero()
        {
            var result = DateTimeRange.FromDuration(Jan1, TimeSpan.Zero);

            result.IsError.Should().BeFalse();
            result.Value.Start.Should().Be(Jan1);
            result.Value.End.Should().Be(Jan1);
        }

        [Fact]
        public void Should_Return_Error_When_Duration_Is_Negative()
        {
            var result = DateTimeRange.FromDuration(Jan1, TimeSpan.FromDays(-1));

            result.IsError.Should().BeTrue();
            result.FirstError.Code.Should().Be("DateTimeRange.Invalid");
            result.FirstError.Description.Should().Be("Duration cannot be negative.");
        }
    }

    public class OverlapsWith
    {
        [Fact]
        public void Should_Return_True_When_Ranges_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Jan15, Mar1).Value;

            var result = range1.OverlapsWith(range2);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_True_When_One_Range_Contains_Another()
        {
            var range1 = DateTimeRange.Create(Jan1, Mar1).Value;
            var range2 = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = range1.OverlapsWith(range2);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_True_When_Touching_At_Endpoint()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Feb1, Mar1).Value;

            var result = range1.OverlapsWith(range2);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_When_Ranges_Do_Not_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Jan15).Value;
            var range2 = DateTimeRange.Create(Feb1, Mar1).Value;

            var result = range1.OverlapsWith(range2);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_True_When_OpenEnded_Overlaps_Closed()
        {
            var openRange = DateTimeRange.Create(Jan15, null).Value;
            var closedRange = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = openRange.OverlapsWith(closedRange);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_When_OpenEnded_Starts_After_Closed_Ends()
        {
            var openRange = DateTimeRange.Create(Feb1, null).Value;
            var closedRange = DateTimeRange.Create(Jan1, Jan15).Value;

            var result = openRange.OverlapsWith(closedRange);

            result.Should().BeFalse();
        }
    }

    public class Contains
    {
        [Fact]
        public void Should_Return_True_When_Date_Is_Inside_Range()
        {
            var range = DateTimeRange.Create(Jan1, Mar1).Value;

            var result = range.Contains(Jan15);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_When_Date_Is_Before_Start()
        {
            var range = DateTimeRange.Create(Jan15, Mar1).Value;

            var result = range.Contains(Jan1);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_False_When_Date_Is_At_End()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.Contains(Feb1);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_True_When_Date_Is_At_Start()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.Contains(Jan1);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_True_When_Date_Is_After_Start_Of_OpenEnded()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            var result = range.Contains(Feb1);

            result.Should().BeTrue();
        }
    }

    public class ContainsRange
    {
        [Fact]
        public void Should_Return_True_When_Other_Range_Is_Fully_Contained()
        {
            var outer = DateTimeRange.Create(Jan1, Mar1).Value;
            var inner = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = outer.ContainsRange(inner);

            result.Should().BeTrue();
        }

        [Fact]
        public void Should_Return_False_When_Other_Range_Extends_Beyond()
        {
            var outer = DateTimeRange.Create(Jan15, Feb1).Value;
            var larger = DateTimeRange.Create(Jan1, Mar1).Value;

            var result = outer.ContainsRange(larger);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_False_When_Other_Starts_Before()
        {
            var outer = DateTimeRange.Create(Jan15, Mar1).Value;
            var inner = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = outer.ContainsRange(inner);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_False_When_Other_Is_OpenEnded_And_Outer_Is_Closed()
        {
            var closed = DateTimeRange.Create(Jan1, Mar1).Value;
            var openEnded = DateTimeRange.Create(Jan15, null).Value;

            var result = closed.ContainsRange(openEnded);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_Return_True_When_Outer_Is_OpenEnded_And_Contains_Closed()
        {
            var openEnded = DateTimeRange.Create(Jan1, null).Value;
            var closed = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = openEnded.ContainsRange(closed);

            result.Should().BeTrue();
        }
    }

    public class IntersectWith
    {
        [Fact]
        public void Should_Return_Intersection_When_Ranges_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Jan15, Mar1).Value;

            var result = range1.IntersectWith(range2);

            result.Should().NotBeNull();
            result!.Start.Should().Be(Jan15);
            result.End.Should().Be(Feb1);
        }

        [Fact]
        public void Should_Return_Null_When_Ranges_Do_Not_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Jan15).Value;
            var range2 = DateTimeRange.Create(Feb1, Mar1).Value;

            var result = range1.IntersectWith(range2);

            result.Should().BeNull();
        }

        [Fact]
        public void Should_Return_Same_Range_When_Identical()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.IntersectWith(range);

            result.Should().NotBeNull();
            result!.Start.Should().Be(Jan1);
            result.End.Should().Be(Feb1);
        }

        [Fact]
        public void Should_Return_Inner_Range_When_One_Contains_Other()
        {
            var outer = DateTimeRange.Create(Jan1, Mar1).Value;
            var inner = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = outer.IntersectWith(inner);

            result.Should().NotBeNull();
            result!.Start.Should().Be(Jan15);
            result.End.Should().Be(Feb1);
        }
    }

    public class GetUnionWith
    {
        [Fact]
        public void Should_Return_Union_When_Ranges_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Jan15, Mar1).Value;

            var result = range1.GetUnionWith(range2);

            result.Start.Should().Be(Jan1);
            result.End.Should().Be(Mar1);
        }

        [Fact]
        public void Should_Return_OpenEnded_Union_When_One_Is_OpenEnded()
        {
            var closed = DateTimeRange.Create(Jan1, Feb1).Value;
            var openEnded = DateTimeRange.Create(Jan15, null).Value;

            var result = closed.GetUnionWith(openEnded);

            result.Start.Should().Be(Jan1);
            result.End.Should().BeNull();
        }

        [Fact]
        public void Should_Throw_When_Ranges_Do_Not_Overlap()
        {
            var range1 = DateTimeRange.Create(Jan1, Jan15).Value;
            var range2 = DateTimeRange.Create(Feb1, Mar1).Value;

            var act = () => range1.GetUnionWith(range2);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage("*non-overlapping*");
        }
    }

    public class Shift
    {
        [Fact]
        public void Should_Shift_Range_By_Positive_TimeSpan()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.Shift(TimeSpan.FromDays(10));

            result.Start.Should().Be(Jan1 + TimeSpan.FromDays(10));
            result.End.Should().Be(Feb1 + TimeSpan.FromDays(10));
        }

        [Fact]
        public void Should_Shift_Range_By_Negative_TimeSpan()
        {
            var range = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = range.Shift(TimeSpan.FromDays(-10));

            result.Start.Should().Be(Jan15 - TimeSpan.FromDays(10));
            result.End.Should().Be(Feb1 - TimeSpan.FromDays(10));
        }

        [Fact]
        public void Should_Shift_OpenEnded_Range()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            var result = range.Shift(TimeSpan.FromDays(5));

            result.Start.Should().Be(Jan1 + TimeSpan.FromDays(5));
            result.End.Should().BeNull();
        }
    }

    public class ShiftDays
    {
        [Fact]
        public void Should_Shift_Range_By_Positive_Days()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.ShiftDays(10);

            result.Start.Should().Be(Jan1 + TimeSpan.FromDays(10));
            result.End.Should().Be(Feb1 + TimeSpan.FromDays(10));
        }

        [Fact]
        public void Should_Shift_OpenEnded_Range_By_Negative_Days()
        {
            var range = DateTimeRange.Create(Jan15, null).Value;

            var result = range.ShiftDays(-5);

            result.Start.Should().Be(Jan15 - TimeSpan.FromDays(5));
            result.End.Should().BeNull();
        }
    }

    public class GetProgress
    {
        [Fact]
        public void Should_Return_Zero_When_AsOf_Is_Before_Start()
        {
            var range = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = range.GetProgress(Jan1);

            result.Should().Be(0);
        }

        [Fact]
        public void Should_Return_Zero_When_AsOf_Is_At_Start()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.GetProgress(Jan1);

            result.Should().Be(0);
        }

        [Fact]
        public void Should_Return_One_When_AsOf_Is_After_End()
        {
            var range = DateTimeRange.Create(Jan1, Jan15).Value;

            var result = range.GetProgress(Feb1);

            result.Should().Be(1);
        }

        [Fact]
        public void Should_Return_One_When_AsOf_Is_At_End()
        {
            var range = DateTimeRange.Create(Jan1, Jan15).Value;

            var result = range.GetProgress(Jan15);

            result.Should().Be(1);
        }

        [Fact]
        public void Should_Return_Half_When_AsOf_Is_Midway()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;
            var midway = Jan1 + (Feb1 - Jan1) / 2;

            var result = range.GetProgress(midway);

            result.Should().BeApproximately(0.5, 0.001);
        }

        [Fact]
        public void Should_Return_NaN_When_Range_Is_OpenEnded()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            var result = range.GetProgress(Feb1);

            result.Should().Be(double.NaN);
        }

        [Fact]
        public void Should_Clamp_Progress_Between_Zero_And_One()
        {
            var range = DateTimeRange.Create(Jan15, Feb1).Value;

            var result = range.GetProgress(Jan1);

            result.Should().Be(0);
        }
    }

    public class StringRepresentation
    {
        [Fact]
        public void Should_Format_Closed_Range()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            var result = range.ToString();

            result.Should().Be($"[{Jan1}, {Feb1})");
        }

        [Fact]
        public void Should_Format_OpenEnded_Range()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            var result = range.ToString();

            result.Should().Be($"[{Jan1}, ∞)");
        }
    }

    public class Equality
    {
        [Fact]
        public void Should_Be_Equal_When_Values_Are_Same()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Jan1, Feb1).Value;

            range1.Should().Be(range2);
            (range1 == range2).Should().BeTrue();
        }

        [Fact]
        public void Should_Not_Be_Equal_When_Values_Differ()
        {
            var range1 = DateTimeRange.Create(Jan1, Feb1).Value;
            var range2 = DateTimeRange.Create(Jan1, Mar1).Value;

            range1.Should().NotBe(range2);
        }

        [Fact]
        public void Should_Be_Equal_When_Both_Are_OpenEnded()
        {
            var range1 = DateTimeRange.Create(Jan1, null).Value;
            var range2 = DateTimeRange.Create(Jan1, null).Value;

            range1.Should().Be(range2);
        }
    }

    public class Properties
    {
        [Fact]
        public void Should_Be_OpenEnded_When_End_Is_Null()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            range.IsOpenEnded.Should().BeTrue();
            range.IsClosed.Should().BeFalse();
        }

        [Fact]
        public void Should_Be_Closed_When_End_Is_Provided()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            range.IsClosed.Should().BeTrue();
            range.IsOpenEnded.Should().BeFalse();
        }

        [Fact]
        public void Should_Have_Duration_When_Range_Is_OpenEnded()
        {
            var range = DateTimeRange.Create(Jan1, null).Value;

            range.HasDuration.Should().BeTrue();
        }

        [Fact]
        public void Should_Have_Duration_When_End_Is_After_Start()
        {
            var range = DateTimeRange.Create(Jan1, Feb1).Value;

            range.HasDuration.Should().BeTrue();
        }

        [Fact]
        public void Should_Not_Have_Duration_When_End_Equals_Start()
        {
            var range = DateTimeRange.Create(Jan1, Jan1).Value;

            range.HasDuration.Should().BeFalse();
        }
    }
}
