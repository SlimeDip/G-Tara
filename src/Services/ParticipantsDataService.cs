using G_Tara.Models;
using System.Text.Json;

namespace G_Tara.Services
{
    public class ParticipantsDataService
    {
        private const string SaveDirectory = "Save Files";
        private const string ParticipantsFileName = "participants.json";
        private string ParticipantsFilePath => Path.Combine(SaveDirectory, ParticipantsFileName);

        public ParticipantsDataService()
        {
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        public List<Participant> LoadParticipants()
        {
            try
            {
                if (File.Exists(ParticipantsFilePath))
                {
                    var json = File.ReadAllText(ParticipantsFilePath);
                    var participants = JsonSerializer.Deserialize<List<Participant>>(json) ?? new List<Participant>();
                    NormalizeAvailability(participants);
                    return participants;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading participants: {ex.Message}");
            }

            return new List<Participant>();
        }

        public void SaveParticipant(Participant participant)
        {
            var participants = LoadParticipants();
            var existingIndex = participants.FindIndex(p => p.Id == participant.Id);

            if (existingIndex >= 0)
            {
                participants[existingIndex] = participant;
            }
            else
            {
                participants.Add(participant);
            }

            NormalizeAvailability(participants);
            SaveAllParticipants(participants);
        }

        public void SaveParticipants(List<Participant> participants)
        {
            NormalizeAvailability(participants);
            SaveAllParticipants(participants);
        }

        public void DeleteParticipant(string id)
        {
            var participants = LoadParticipants();
            participants.RemoveAll(p => p.Id == id);
            SaveAllParticipants(participants);
        }

        public Participant? GetParticipantById(string id)
        {
            var participants = LoadParticipants();
            return participants.FirstOrDefault(p => p.Id == id);
        }

        public List<Participant> SearchParticipants(string searchTerm)
        {
            var participants = LoadParticipants();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return participants;
            }

            var term = searchTerm.ToLower();
            return participants.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Email.ToLower().Contains(term)
            ).ToList();
        }

        private void NormalizeAvailability(List<Participant> participants)
        {
            foreach (var participant in participants)
            {
                if (participant.AvailableDates == null)
                {
                    participant.AvailableDates = new List<DateTime>();
                }

                if (participant.AvailableDates.Count == 0 && participant.AvailableStartDate != default && participant.AvailableEndDate != default)
                {
                    var start = participant.AvailableStartDate.Date;
                    var end = participant.AvailableEndDate.Date;
                    if (end < start)
                    {
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    var dates = new List<DateTime>();
                    for (var day = start; day <= end; day = day.AddDays(1))
                    {
                        dates.Add(day);
                    }

                    participant.AvailableDates = dates;
                }
            }
        }

        private void SaveAllParticipants(List<Participant> participants)
        {
            try
            {
                var json = JsonSerializer.Serialize(participants, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ParticipantsFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving participants: {ex.Message}");

            }
        }
    }
}
