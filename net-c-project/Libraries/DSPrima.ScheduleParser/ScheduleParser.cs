using DSPrima.ScheduleParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DSPrima.ScheduleParser
{
    /// <summary>
    /// Parses a schedule.
    /// // TODO Specify schedule documentation
    /// </summary>
    public class ScheduleParser
    {
        /// <summary>
        /// Gets or sets the injections that are available
        /// </summary>
        private Dictionary<string, IEnumerable<DateTime>> Injections { get; set; }

        /// <summary>
        /// Parses a given schedule for dates against the given list of milestones
        /// </summary>
        /// <param name="schedule">The full schedule string to parse</param>
        /// <param name="startTime">The time to start calculating from. This may be replaced by milestones but must be given.</param>
        /// <param name="injections">The dictionary of Milestone and the milestone dates. The dates will be parsed in the order provided</param>
        /// <returns>The list of DateTimes found.</returns>
        public Dictionary<string, List<DateTime>> ParseSchedule(string schedule, DateTime startTime, Dictionary<string, IEnumerable<DateTime>> injections)
        {
            this.Injections = injections;

            List<DateTime> dates = new List<DateTime>();
            string[] lines = schedule.Replace("\r", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            DateTime start = startTime;
            Dictionary<string, List<DateTime>> finalResult = new Dictionary<string, List<DateTime>>();
            foreach (string line in lines)
            {
                DateTime[] result = this.ProcessLine(line, start);
                if (result == null || result.Length == 0)
                {
                    // We are done now, without the current line calculated to a date, the next lines can't be calculated.
                    // TODO Or are we?                    
                    finalResult.Add(line, new List<DateTime>());
                }
                else
                {
                    finalResult.Add(line, result.ToList());
                    start = result[result.Length - 1];
                }
            }

            return finalResult;
        }

        /// <summary>
        /// Processes the current line with the start date being the given date
        /// </summary>
        /// <param name="line">The line being processed</param>
        /// <param name="start">The start date to use for calculations</param>
        /// <returns>An array with all DateTime entries calculated</returns>
        private DateTime[] ProcessLine(string line, DateTime start)
        {
            // Find order to devide in sections
            string[] parts = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            List<PartStorage[]> sections = new List<PartStorage[]>();

            // Order of importants:
            // Not used anymore
            /*
             * Y = 4
             * M = 3 
             * W = 2
             * D = 1
            */

            // First generate sections for the current line            
            // List<PartStorage> CurrentSection = new List<PartStorage>();
            foreach (string p in parts)
            {
                PartStorage ps = this.DeterminePart(p);
                sections.Add(new PartStorage[] { ps });
                /*if (CurrentSection.Count > 0 && CurrentSection[CurrentSection.Count - 1].Type <= ps.Type)
                {
                    sections.Add(CurrentSection.ToArray());
                    CurrentSection.Clear();
                }

                CurrentSection.Add(ps);
                 */
            }

            // if (CurrentSection.Count > 0) sections.Add(CurrentSection.ToArray());
            // CurrentSection.Clear();

            // Process each section to see if we can find a date
            List<DateTime> results = new List<DateTime>();
            IEnumerable<DateTime> startDates = new DateTime[] { start };
            foreach (PartStorage[] section in sections)
            {
                List<DateTime> internalResult = new List<DateTime>();
                foreach (PartStorage part in section)
                {
                    if (part.Type == PartType.Injection)
                    {
                        var d = this.ProcessSectionPart(part);
                        if (d == null || d.Count() == 0) return null;
                        if (sections.Count == 1)
                        {
                            results.AddRange(d);
                        }
                        else
                        {
                            startDates = d;
                        }
                    }
                    else if (part.Type == PartType.FullDate)
                    {
                        var d = this.ProcessSectionPart(part);
                        if (sections.Count > 1 && d.Count() > 0)
                        {
                            startDates = d;
                        }
                        else
                        {
                            results.AddRange(d);
                        }
                    }
                    else
                    {
                        if (internalResult.Count == 0)
                        {
                            internalResult = this.ProcessSectionPart(part, startDates);
                        }
                        else
                        {
                            internalResult = this.ProcessSectionPart(part, internalResult);
                        }
                    }
                }

                results.AddRange(internalResult);
            }

            results.Sort();

            return results.ToArray();
        }

        /*
{
    +1m +2d +2m +2d
    +2w +3d
    +3m +1m
    +3m*2
}";
         */

        /// <summary>
        /// Calculates the Date for the given PartStorage
        /// If it is a full date that is being parsed and returned
        /// If it is an Injection, it is found in the list of injections
        /// </summary>
        /// <param name="sectionPart">The PartStorage to process</param>
        /// <returns>A datetime if found, null otherwise</returns>
        private IEnumerable<DateTime> ProcessSectionPart(PartStorage sectionPart)
        {
            switch (sectionPart.Type)
            {
                case PartType.FullDate:
                    return new DateTime[] { DateTime.Parse(sectionPart.Value) };
                case PartType.Injection:
                    if (this.Injections.ContainsKey(sectionPart.Injection))
                    {
                        if (sectionPart.Indexes.Count > 0)
                        {
                            DateTime[] data = this.Injections[sectionPart.Injection].ToArray();
                            List<DateTime> result = new List<DateTime>();
                            foreach (int index in sectionPart.Indexes)
                            {
                                if (index < data.Length) result.Add(data[index]);
                            }

                            return result;
                        }
                        else
                        {
                            return this.Injections[sectionPart.Injection];
                        }
                    }

                    return null;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Processes a section part for all given start times.
        /// </summary>
        /// <param name="sectionPart">The Part storage to process</param>
        /// <param name="startTimes">The start times to process against. The result is again all start times passed</param>
        /// <returns>The list of DateTime entries calculated. This list is at least as big as the number of start times provided</returns>
        private List<DateTime> ProcessSectionPart(PartStorage sectionPart, IEnumerable<DateTime> startTimes)
        {
            List<DateTime> result = new List<DateTime>();
            bool add = !sectionPart.Value.StartsWith("-");
            string[] parts;
            if (sectionPart.Value.Contains("*"))
            {
                parts = sectionPart.Value.Split(new char[] { '*' });
            }
            else
            {
                parts = new string[] { sectionPart.Value };
            }

            int repeatCount = parts.Length == 2 ? int.Parse(parts[1]) : 1;
            foreach (DateTime start in startTimes)
            {
                for (int i = 0; i < repeatCount; i++)
                {
                    string valueString = parts[0].StartsWith("-") || parts[0].StartsWith("+") ? parts[0].Substring(1, parts[0].Length - 2) : parts[0].Substring(0, parts[0].Length - 2);
                    int value = add ? int.Parse(valueString) : 0 - int.Parse(valueString);
                    value = value * (i + 1);
                    switch (sectionPart.Type)
                    {
                        case PartType.Year:
                            result.Add(start.AddYears(value));
                            break;
                        case PartType.Month:
                            result.Add(start.AddMonths(value));
                            break;
                        case PartType.Week:
                            result.Add(start.AddDays(value * 7));
                            break;
                        case PartType.Day:
                            result.Add(start.AddDays(value));
                            break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Determines what type of part the given string is.
        /// </summary>
        /// <param name="p">The string to determin what it is</param>
        /// <returns>The PartStorage with the string and what type of data it is expected to be</returns>
        private PartStorage DeterminePart(string p)
        {
            if (p.StartsWith("%"))
            {
                var valueFinder = new Regex(@"%.*?%");
                Regex indexFinder = new Regex(@"\[.?\]");
                var indexes = indexFinder.Matches(p).Cast<Match>().Select(m => int.Parse(m.Value.Replace("[", string.Empty).Replace("]", string.Empty)) - 1).ToList();

                return new PartStorage() { Type = PartType.Injection, Value = p, Injection = valueFinder.Match(p).Value.ToLower().Replace("%", string.Empty), Indexes = indexes, Date = null, OwnSection = true };
            }

            DateTime result = new DateTime();
            if (DateTime.TryParse(p, out result)) return new PartStorage() { Type = PartType.FullDate, Value = p, Date = result, OwnSection = true };
            PartType type;
            string part = p.ToUpper();
            if (part.Contains("Y")) type = PartType.Year;
            else if (part.Contains("M")) type = PartType.Month;
            else if (part.Contains("W")) type = PartType.Week;
            else type = PartType.Day;

            // TODO what to do with Unknown types?
            return new PartStorage() { Type = type, Value = p, OwnSection = false, Date = null };
        }
    }
}
