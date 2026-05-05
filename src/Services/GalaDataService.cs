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

            var (newStart, newEnd) = GetGalaDateRange(newGala);

            return galas.Any(g =>
                g.Id != newGala.Id &&
                RangesOverlap(newStart, newEnd, GetGalaDateRange(g))
            );
        }

        public List<Gala> GetDateConflicts(Gala newGala) //Lists the conflicting Schedules
        {
            var galas = LoadGalas();

            var (newStart, newEnd) = GetGalaDateRange(newGala);

            return galas.Where(g =>
                g.Id != newGala.Id &&
                RangesOverlap(newStart, newEnd, GetGalaDateRange(g))
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

            var (newStart, newEnd) = GetGalaDateRange(gala);

            var conflictingGala = galas.FirstOrDefault(g =>
                g.Id != gala.Id &&
                RangesOverlap(newStart, newEnd, GetGalaDateRange(g))
            );

            if (conflictingGala != null) //Added: Throws a specific exception if a conflict is detected
            {
                throw new InvalidOperationException(
                    $"Cannot save gala scheduled for {gala.ScheduledDate:yyyy-MM-dd}. " +
                    $"It conflicts with existing gala '{conflictingGala.Name}' (Id: {conflictingGala.Id}) on the same date.");
            }

            var conflicts = galas.Where(g =>
                g.Id != gala.Id &&
                RangesOverlap(newStart, newEnd, GetGalaDateRange(g))
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

        private static (DateTime Start, DateTime End) GetGalaDateRange(Gala gala)
        {
            var start = gala.RangeStartDate != default
                ? gala.RangeStartDate.Date
                : gala.ScheduledDate.Date;
            var end = gala.RangeEndDate != default
                ? gala.RangeEndDate.Date
                : start;

            if (end < start)
            {
                var temp = start;
                start = end;
                end = temp;
            }

            return (start, end);
        }

        private static bool RangesOverlap(DateTime startA, DateTime endA, (DateTime Start, DateTime End) rangeB)
        {
            return startA <= rangeB.End && endA >= rangeB.Start;
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
