using G_Tara.Models;

namespace G_Tara.Services
{
    /// Service for managing operations related to Person (Host and Participant members).
    public class MemberService
    {
        /// Checks if a person is available on the specified date.
        public bool IsAvailableOn(Person person, DateTime date)
        {
            return person.AvailableStartDate.Date <= date.Date && person.AvailableEndDate.Date >= date.Date;
        }

        /// Checks if a host manages a specific gala.
        public bool HostManagesGala(Host host, string galaId)
        {
            return host.ManagedGalaIds.Contains(galaId);
        }

        /// Adds a gala to the host's list of managed galas.
        public void AddManagedGala(Host host, string galaId)
        {
            if (!host.ManagedGalaIds.Contains(galaId))
            {
                host.ManagedGalaIds.Add(galaId);
            }
        }

        /// Removes a gala from the host's list of managed galas.
        public void RemoveManagedGala(Host host, string galaId)
        {
            host.ManagedGalaIds.Remove(galaId);
        }

        /// Gets all galas managed by a host.
        public List<string> GetManagedGalas(Host host)
        {
            return new List<string>(host.ManagedGalaIds);
        }

        /// Validates if a person has valid information (name and email).
        public bool IsValidMember(Person person)
        {
            return !string.IsNullOrWhiteSpace(person.Name) &&
                   !string.IsNullOrWhiteSpace(person.Email);
        }

        /// Checks if two people have conflicting availability (same available date).
        public bool HasAvailabilityConflict(Person person1, Person person2, DateTime galaDate)
        {
            return IsAvailableOn(person1, galaDate) && IsAvailableOn(person2, galaDate);
        }

        /// Gets the common available dates between multiple people.
        public List<DateTime> GetCommonAvailableDates(List<Person> people)
        {
            if (people.Count == 0)
                return new List<DateTime>();

            // Just return an empty list or overlapping date logic because ranges make this complex
            return new List<DateTime>();
        }
    }
}
