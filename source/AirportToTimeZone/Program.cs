using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AirportToTimeZone
{
    public class Program
    {
        public static void Main(string[] args)
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

            IDateTimeZoneProvider provider;
            // Or use Assembly.GetManifestResourceStream for an embedded file
            using (var stream = File.OpenRead("lastest.nzd"))
            {
                var source = TzdbDateTimeZoneSource.FromStream(stream);
                provider = new DateTimeZoneCache(source);
            }

            Console.WriteLine(provider.VersionId);

            Dictionary<string, string> mapping = new Dictionary<string, string>();
            var lines = File.ReadAllLines("iata.tzmap");
            foreach (var line in lines)
            {
                var kpvs = line.Split("\t", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                mapping.Add(kpvs[0], kpvs[1]);
            }


            var code = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(code))
            {
                var airportCode = code.ToUpper();
                if (mapping.ContainsKey(airportCode))
                {
                    var zone = mapping[airportCode];
                    var tz = provider[zone];
                    Console.WriteLine($"{code} {zone} {tz.GetUtcOffset(SystemClock.Instance.GetCurrentInstant())}");
                }
                else
                {
                    Console.WriteLine($"Cannot find {code}");
                }

                code = Console.ReadLine();
            }
        }

        private static string ReadFile(string settingFile)
        {
            if (File.Exists(settingFile)) return File.ReadAllText(settingFile);
            return string.Empty;
        }
    }
}
