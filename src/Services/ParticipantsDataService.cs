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
                    return JsonSerializer.Deserialize<List<Participant>>(json) ?? new List<Participant>();
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

            SaveAllParticipants(participants);
        }

        public void SaveParticipants(List<Participant> participants)
        {
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
