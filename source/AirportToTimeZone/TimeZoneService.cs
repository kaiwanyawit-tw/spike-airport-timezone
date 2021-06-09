using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AirportToTimeZone
{
    public class TimeZoneService : ITimeZoneService
    {
        private readonly IDateTimeZoneProvider _provider;
        private readonly Dictionary<string, string> _mapping = new();

        public TimeZoneService()
        {
            var settingFile = "setting.txt";
            var cacheFile = ReadFile(settingFile);

            using (var client = new WebClient())
            {
                var url = "https://nodatime.org/tzdb/latest.txt";
                var fileLocation = client.DownloadString(url);

                if (fileLocation != cacheFile)
                {
                    client.DownloadFile(fileLocation, "lastest.nzd");
                    File.WriteAllText(settingFile, cacheFile);
                }

                if (!File.Exists("iata.tzmap"))
                {
                    client.DownloadFile("https://raw.githubusercontent.com/hroptatyr/dateutils/tzmaps/iata.tzmap",
                        "iata.tzmap");
                }
            }

            // Or use Assembly.GetManifestResourceStream for an embedded file
            using (var stream = File.OpenRead("lastest.nzd"))
            {
                var source = TzdbDateTimeZoneSource.FromStream(stream);
                _provider = new DateTimeZoneCache(source);
            }

            var lines = File.ReadAllLines("iata.tzmap");
            foreach (var line in lines)
            {
                var kpvs = line.Split("\t", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                _mapping.Add(kpvs[0], kpvs[1]);
            }
        }

        public (string timeZone, Offset offset) FromAirportCode(string code)
        {
            var result = _provider[_mapping[code]];
            return (result.ToString(), result.GetUtcOffset(SystemClock.Instance.GetCurrentInstant()));
        }

        public bool ContainsAirportCode(string code)
        {
            return _mapping.ContainsKey(code);
        }

        private string ReadFile(string settingFile)
        {
            if (File.Exists(settingFile)) return File.ReadAllText(settingFile);
            return string.Empty;
        }
    }
}