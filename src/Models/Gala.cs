namespace G_Tara.Models
{
    public abstract class Person
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime AvailableStartDate { get; set; }
        public DateTime AvailableEndDate { get; set; }
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
            return GetAllAttendees().Where(p => p.AvailableStartDate.Date <= ScheduledDate.Date && p.AvailableEndDate.Date >= ScheduledDate.Date).ToList();
        }

        public List<Participant> GetAvailableParticipants()
        {
            return Participants.Where(p => p.AvailableStartDate.Date <= ScheduledDate.Date && p.AvailableEndDate.Date >= ScheduledDate.Date).ToList();
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
    }

    public class Host : Person
    {
        public List<string> ManagedGalaIds { get; set; } = new();
        // Wala pa to kwenta as of now
    }
}
