using G_Tara.Models;
using System.Text.Json;

namespace G_Tara.Services
{
    public class GalaDataService
    {
        private readonly string _dataPath;
        private readonly string _dataFile;

        public bool HasDateConflict(Gala newGala) //Added: Checks if there are conflict before saving a new Gala
        {
            var galas = LoadGalas();

            return galas.Any(g =>
                g.Id != newGala.Id &&
                g.ScheduledDate.Date == newGala.ScheduledDate.Date
            );
        }

        public List<Gala> GetDateConflicts(Gala newGala) //Lists the conflicting Schedules
        {
            var galas = LoadGalas();

            return galas.Where(g =>
                g.Id != newGala.Id &&
                g.ScheduledDate.Date == newGala.ScheduledDate.Date
            ).ToList();
        }

        public GalaDataService()
        {
            var saveDir = Environment.GetEnvironmentVariable("SAVE_DIR") ?? "Save Files";
            var baseDir = Directory.GetCurrentDirectory();
            _dataPath = Path.Combine(baseDir, saveDir);
            _dataFile = Path.Combine(_dataPath, "galas.json");

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
        }

        public List<Gala> LoadGalas()
        {
            if (!File.Exists(_dataFile))
            {
                return new List<Gala>();
            }

            try
            {
                var json = File.ReadAllText(_dataFile);
                return JsonSerializer.Deserialize<List<Gala>>(json) ?? new List<Gala>();
            }
            catch
            {
                return new List<Gala>();
            }
        }

        public void SaveGalas(List<Gala> galas)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(galas, options);
            File.WriteAllText(_dataFile, json);
        }

        public List<Gala> SaveGala(Gala gala) // Modified to return schedule overlap warnings
        {
            var galas = LoadGalas();

            var conflictingGala = galas.FirstOrDefault(g =>
                g.Id != gala.Id &&
                g.ScheduledDate.Date == gala.ScheduledDate.Date
            );

            if (conflictingGala != null) //Added: Throws a specific exception if a conflict is detected
            {
                throw new InvalidOperationException(
                    $"Cannot save gala scheduled for {gala.ScheduledDate:yyyy-MM-dd}. " +
                    $"It conflicts with existing gala '{conflictingGala.Name}' (Id: {conflictingGala.Id}) on the same date.");
            }

            var conflicts = galas.Where(g =>
                g.Id != gala.Id &&
                g.ScheduledDate.Date == gala.ScheduledDate.Date
            ).ToList();

            var existing = galas.FirstOrDefault(g => g.Id == gala.Id);

            if (existing != null)
            {
                galas.Remove(existing);
            }

            galas.Add(gala);
            SaveGalas(galas);

            return conflicts;
        }

        public void DeleteGala(string id)
        {
            var galas = LoadGalas();
            galas.RemoveAll(g => g.Id == id);
            SaveGalas(galas);
        }

        public Gala? GetGala(string id)
        {
            var galas = LoadGalas();
            return galas.FirstOrDefault(g => g.Id == id);
        }
    }
}
