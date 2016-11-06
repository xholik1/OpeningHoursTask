using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpeningHoursTask
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<SeasonSpan> seasonalSpans = new List<SeasonSpan>()
            {
                new SeasonSpan()
                {
                    StartOfSeason = new DateTime(1,1,1),
                    EndOfSeason = new DateTime(1,3,31),
                    OpeningHours = new OpeningHours(new DateTime(1,1,1,9,0,0),new DateTime(1,1,1,15,0,0), 
                    new LunchBreak()
                    {
                        StartOfLunchBreak = new DateTime(1,1,1,11,0,0),
                        EndOfLunchBreak = new DateTime(1,1,1,12,0,0)
                    })
                }
                ,
                new SeasonSpan()
                {
                    StartOfSeason = new DateTime(1,4,1),
                    EndOfSeason = new DateTime(1,8,31),
                    OpeningHours = new OpeningHours(new DateTime(1,1,1,8,0,0),new DateTime(1,1,1,17,0,0), null)
                },
                new SeasonSpan()
                {
                    StartOfSeason = new DateTime(1,9,1),
                    EndOfSeason = new DateTime(1,9,30),
                    OpeningHours = new OpeningHours()
                },
                new SeasonSpan()
                {
                    StartOfSeason = new DateTime(1,10,1),
                    EndOfSeason = new DateTime(1,12,31),
                    OpeningHours = new OpeningHours(new DateTime(1,1,1,9,0,0),new DateTime(1,1,1,12,0,0), null)
                },
            };

            IEnumerable<DailyRule> dailyRules = new List<DailyRule>()
            {
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,1,1),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,5,1),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,5,8),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,7,5),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,7,6),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,9,28),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,10,28),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,11,17),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,12,24),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,12,25),
                    OpeningHours = new OpeningHours()
                },
                new DailyRule()
                {
                    DayOfRule = new DateTime(1,12,26),
                    OpeningHours = new OpeningHours()
                }
            };

            IEnumerable<WeekDayRule> weekDayRules = new List<WeekDayRule>() {
                new WeekDayRule()
                {
                    DayOfRule = DayOfWeek.Sunday,
                    OpeningHours = new OpeningHours()
                },
                new WeekDayRule()
                {
                    DayOfRule = DayOfWeek.Saturday,
                    OpeningHours = new OpeningHours(new DateTime(1,1,1,0,0,0), new DateTime(1,1,1,12,0,0), null)
                }
            };

            var openingHoursHandler = new OpeningHoursHandler(seasonalSpans, dailyRules, weekDayRules);

            Console.WriteLine("January to March");
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 1, 1)));
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 1, 2)));
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 3, 31)));

            Console.WriteLine("\nApril to August");
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 5, 8)));
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 4, 2)));

            Console.WriteLine("\nSeptember");
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 9, 28)));
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 9, 1)));

            Console.WriteLine("\nOctober to December");
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 10, 28)));
            Console.WriteLine(openingHoursHandler.OpeningHoursForGivenDate(new DateTime(1, 10, 1)));

            Console.WriteLine(openingHoursHandler.IsItOpen().ToString());
            Console.ReadLine();
        }
    }
}
