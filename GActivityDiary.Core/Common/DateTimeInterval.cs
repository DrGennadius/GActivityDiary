using System;

namespace GActivityDiary.Core.Common
{
    /// <summary>
    /// <see cref="DateTime"/> interval.
    /// </summary>
    public struct DateTimeInterval
    {
        public DateTime Start;
        public DateTime End;

        public DateTimeInterval(DateTime dateTime)
            : this(dateTime.Date, dateTime.Date.AddDays(1))
        {
        }

        public DateTimeInterval(DateTime dateTime, TimeSpan timeSpan)
            : this(dateTime, dateTime.Add(timeSpan))
        {
        }

        public DateTimeInterval(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object obj)
        {
            return obj is DateTimeInterval other &&
                   Start == other.Start &&
                   End == other.End;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        public void Deconstruct(out DateTime start, out DateTime end)
        {
            start = Start;
            end = End;
        }

        public static implicit operator (DateTime, DateTime)(DateTimeInterval value)
        {
            return (value.Start, value.End);
        }

        public static implicit operator DateTimeInterval((DateTime, DateTime) value)
        {
            return new DateTimeInterval(value.Item1, value.Item2);
        }

        public bool IsIntersected(DateTimeInterval other)
        {
            // (x1 < y2) && (x2 < y1)
            return Start < other.End && other.Start < End;
        }

        public bool IsIntersected(DateTimeInterval? other)
        {
            if (other is null)
            {
                return false;
            }
            return IsIntersected(other.Value);
        }

        public bool IsWithin(DateTimeInterval other)
        {
            // (x1 >= x2) && (y1 <= y2) && (x1 < y2) && (x2 < y1)
            return Start >= other.Start
                   && End <= other.End
                   && Start < other.End
                   && other.Start < End;
        }

        public bool IsWithin(DateTimeInterval? other)
        {
            if (other is null)
            {
                return false;
            }
            return IsWithin(other.Value);
        }

        public static DateTimeInterval CreateFromDays(DateTime dayFrom, DateTime dayTo)
        {
            return new DateTimeInterval(dayFrom, dayTo.AddDays(1));
        }

        public override string ToString()
        {
            return $"({Start}, {End})";
        }
    }
}
