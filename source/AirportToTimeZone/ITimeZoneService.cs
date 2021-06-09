using NodaTime;

namespace AirportToTimeZone
{
    internal interface ITimeZoneService
    {
        bool ContainsAirportCode(string airportCode);

        (string timeZone, Offset offset) FromAirportCode(string airportCode);
    }
}