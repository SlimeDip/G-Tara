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
        public virtual string DisplayName => Name;
        public virtual string GetEmailGreeting()
        {
            return string.IsNullOrWhiteSpace(Name) ? "Hello!" : $"Hello, {Name}!";
        }

        public virtual string GetEmailSignature()
        {
            return string.IsNullOrWhiteSpace(Name) ? "Organizer" : $"Organizer: {Name}";
        }

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
        public DateTime RangeStartDate { get; set; }
        public DateTime RangeEndDate { get; set; }
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
        public override string DisplayName => string.IsNullOrWhiteSpace(Name) ? "Participant" : Name;

        public override string GetEmailGreeting()
        {
            return string.IsNullOrWhiteSpace(Name) ? "Tara na!" : $"Tara na, {Name}!";
        }

        public string ImagePath { get; set; } = string.Empty;
        public string AvailableDatesDisplay
        {
            get
            {
                if (AvailableDates.Count > 0)
                {
                    var ordered = AvailableDates.Select(d => d.Date).Distinct().OrderBy(d => d).ToList();
                    var first = ordered[0];
                    var last = ordered[ordered.Count - 1];
                    var range = first == last
                        ? first.ToString("MMM d, yyyy")
                        : $"{first:MMM d, yyyy} - {last:MMM d, yyyy}";
                    return ordered.Count == 1
                        ? range
                        : $"{ordered.Count} dates ({range})";
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
        public override string DisplayName => string.IsNullOrWhiteSpace(Name) ? "Host" : $"{Name} (Host)";

        public override string GetEmailSignature()
        {
            return string.IsNullOrWhiteSpace(Name) ? "Organizer" : $"Organizer: {Name}";
        }

        public List<string> ManagedGalaIds { get; set; } = new();
    }
}
