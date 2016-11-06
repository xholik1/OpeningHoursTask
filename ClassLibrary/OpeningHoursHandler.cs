using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class OpeningHoursHandler 
    {
        private IEnumerable<SeasonSpan> _seasonalSpans;

        private IEnumerable<DailyRule> _dailyRules;

        private IEnumerable<WeekDayRule> _weekDayRules;

        public OpeningHoursHandler(IEnumerable<SeasonSpan> seasonalSpans, IEnumerable<DailyRule> dailyRules, IEnumerable<WeekDayRule> weekDayRules)
        {
            _seasonalSpans = seasonalSpans;
            _dailyRules = dailyRules;
            _weekDayRules = weekDayRules;
        }

        private OpeningHours GetSeasonSpan(DateTime date)
        {
            return _seasonalSpans.Where(x => x.StartOfSeason <= date && x.EndOfSeason >= date).Single().OpeningHours;
        }

        private OpeningHours GetDailyRule(DateTime date)
        {
            var dailyRule = _dailyRules.Where(x => x.DayOfRule.Date.Equals(date.Date)).SingleOrDefault();
            return dailyRule == null ? null : dailyRule.OpeningHours;
        }

        private OpeningHours GetWeekDayRule(DateTime date)
        {
            var weekDayRule = _weekDayRules.Where(x => x.DayOfRule == date.DayOfWeek).SingleOrDefault();
            return weekDayRule == null ? null : weekDayRule.OpeningHours;
        }

        public OpeningHours OpeningHoursForGivenDate(DateTime dateTime)
        {
            var date = new DateTime(1, dateTime.Month, dateTime.Day);

            var SeasonalOpeningHours = GetSeasonSpan(date);
            var OpeningHoursFromDailyRule = GetDailyRule(date);
            var OpeningHoursFromWeekDayRule = GetWeekDayRule(date);

            var SeasonalAndDailyOpeningHours = OpeningHoursFromDailyRule != null ? SeasonalOpeningHours.MergeOpeningHours(OpeningHoursFromDailyRule) : SeasonalOpeningHours;
            var FinalOpeningHours = OpeningHoursFromWeekDayRule != null ? SeasonalAndDailyOpeningHours.MergeOpeningHours(OpeningHoursFromWeekDayRule) : SeasonalAndDailyOpeningHours;

            return FinalOpeningHours;
        }

        public bool IsItOpen()
        {           
            return OpeningHoursForGivenDate(DateTime.Now).IsOpen(DateTime.Now);
        }
    }

    public class SeasonSpan
    {
        public DateTime StartOfSeason { get; set; }

        public DateTime EndOfSeason { get; set; }

        public OpeningHours OpeningHours { get; set; }
    } 

    public class DailyRule
    {
        public DateTime DayOfRule { get; set; }

        public OpeningHours OpeningHours { get; set; }

    }

    public class WeekDayRule
    {
        public DayOfWeek DayOfRule { get; set; }

        public OpeningHours OpeningHours { get; set; }
    }

    public class OpeningHours
    {
        public DateTime? OpeningTime { get; }

        public DateTime? ClosingTime { get; }

        public LunchBreak LunchBreak { get; }
        
        public bool IsClosed()
        {
            return OpeningTime == null && ClosingTime == null;
        }

        public bool IsOpen(DateTime dateTime)
        {
            var time = new DateTime(1, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second);
            return IsClosed() ? false : (OpeningTime <= time && ClosingTime >= time) && isNotLunchBreakNow(time);
        }

        private bool isNotLunchBreakNow(DateTime time)
        {
            return LunchBreak == null || (LunchBreak.StartOfLunchBreak > time || LunchBreak.EndOfLunchBreak < time);
        }

        public OpeningHours()
        {
        }

        public OpeningHours(DateTime openingTime, DateTime closingTime, LunchBreak lunchBreak)
        {
            OpeningTime = openingTime;
            ClosingTime = closingTime;
            LunchBreak = lunchBreak;
        }

        public override string ToString()
        {
            return string.Format("Opening time {0}\nClosing time {1}\nLunch break {2} - {3}",
                IsClosed() ? "CLOSED" : ((DateTime) OpeningTime).ToString("HH:mm"),
                IsClosed() ? "CLOSED" : ((DateTime) ClosingTime).ToString("HH:mm"),
                LunchBreak != null ? LunchBreak.StartOfLunchBreak.ToString("HH:mm") : "N/A",
                LunchBreak != null ? LunchBreak.EndOfLunchBreak.ToString("HH:mm") : "N/A");
        }

        public OpeningHours MergeOpeningHours(OpeningHours openingHours)
        {
            if (openingHours.IsClosed()) return openingHours;
            if (IsClosed()) return this;

            var openingTime = OpeningTime > openingHours.OpeningTime ? OpeningTime : openingHours.OpeningTime;
            var closingTime = ClosingTime > openingHours.ClosingTime ? openingHours.ClosingTime : ClosingTime;

            return new OpeningHours((DateTime) openingTime, (DateTime) closingTime, null);                
        }
    }

    public class LunchBreak
    {
        public DateTime StartOfLunchBreak { get; set; }

        public DateTime EndOfLunchBreak { get; set; }
    }
}
