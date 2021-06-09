using System;

namespace AirportToTimeZone
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ITimeZoneService service = new TimeZoneService();

            var code = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(code))
            {
                var airportCode = code.ToUpper();
                if (service.ContainsAirportCode(airportCode))
                {
                    var (zone, offset) = service.FromAirportCode(airportCode);
                    Console.WriteLine($"{code} {zone} {offset}");
                }
                else
                {
                    Console.WriteLine($"Cannot find {code}");
                }

                code = Console.ReadLine();
            }
        }
    }
}