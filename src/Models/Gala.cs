namespace G_Tara.Models
{
    public abstract class Person
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<DateTime> AvailableDates { get; set; } = new();
        public DateTime AvailableStartDate { get; set; }
        public DateTime AvailableEndDate { get; set; }

        public bool IsAvailableOn(DateTime date)
        {
            if (AvailableDates.Count > 0)
            {
                return AvailableDates.Any(d => d.Date == date.Date);
            }

            if (AvailableStartDate != default || AvailableEndDate != default)
            {
                return AvailableStartDate.Date <= date.Date && AvailableEndDate.Date >= date.Date;
            }

            return false;
        }
    }

    public class Gala
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public DateTime ScheduledDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Status { get; set; } = "Planned";
        public string Plan { get; set; } = string.Empty;
        public List<LocationItem> LocationItems { get; set; } = new();
        public WeatherData? Weather { get; set; }
        public string HostName { get; set; } = string.Empty;
        public List<Participant> Participants { get; set; } = new();

        public List<Person> GetAllAttendees()
        {
            var attendees = new List<Person>();
            attendees.AddRange(Participants);
            return attendees;
        }

        public List<Person> GetAvailableAttendees()
        {
            return GetAllAttendees().Where(p => p.IsAvailableOn(ScheduledDate)).ToList();
        }

        public List<Participant> GetAvailableParticipants()
        {
            return Participants.Where(p => p.IsAvailableOn(ScheduledDate)).ToList();
        }

        public bool HasOverlappingAvailability()
        {
            return GetAvailableAttendees().Count > 1;
        }
    }

    public class LocationItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; } = string.Empty;
    }

    public class WeatherData
    {
        public double Temperature { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string IconUrl { get; set; } = string.Empty;
    }

    public class Participant : Person
    {
        // Yooo pls magisip kayo ng something dito

        public string AvailableDatesDisplay
        {
            get
            {
                if (AvailableDates.Count > 0)
                {
                    var ordered = AvailableDates.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
                    var shown = ordered.Take(5).Select(d => d.ToString("yyyy-MM-dd")).ToList();
                    if (ordered.Count > shown.Count)
                    {
                        shown.Add($"+{ordered.Count - shown.Count} more");
                    }
                    return string.Join(", ", shown);
                }

                if (AvailableStartDate != default || AvailableEndDate != default)
                {
                    return $"{AvailableStartDate:yyyy-MM-dd} to {AvailableEndDate:yyyy-MM-dd}";
                }

                return "None";
            }
        }
    }

    public class Host : Person
    {
        public List<string> ManagedGalaIds { get; set; } = new();
        // Wala pa to kwenta as of now
    }
}
