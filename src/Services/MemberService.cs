using G_Tara.Models;

namespace G_Tara.Services
{
    public class MemberService
    {
        public bool IsAvailableOn(Person person, DateTime date)
        {
            return person.IsAvailableOn(date);
        }

        public bool HostManagesGala(Host host, string galaId)
        {
            return host.ManagedGalaIds.Contains(galaId);
        }

        public void AddManagedGala(Host host, string galaId)
        {
            if (!host.ManagedGalaIds.Contains(galaId))
            {
                host.ManagedGalaIds.Add(galaId);
            }
        }

        public void RemoveManagedGala(Host host, string galaId)
        {
            host.ManagedGalaIds.Remove(galaId);
        }

        public List<string> GetManagedGalas(Host host)
        {
            return new List<string>(host.ManagedGalaIds);
        }

        public bool IsValidMember(Person person)
        {
            return !string.IsNullOrWhiteSpace(person.Name) &&
                   !string.IsNullOrWhiteSpace(person.Email);
        }

        public bool HasAvailabilityConflict(Person person1, Person person2, DateTime galaDate)
        {
            return IsAvailableOn(person1, galaDate) && IsAvailableOn(person2, galaDate);
        }

        public List<DateTime> GetCommonAvailableDates(List<Person> people)
        {
            if (people.Count == 0)
                return new List<DateTime>();

            return new List<DateTime>();
        }
    }
}
