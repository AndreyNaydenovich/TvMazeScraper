using Newtonsoft.Json.Converters;

namespace TvMazeScraper.Presentation.Helpers
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}