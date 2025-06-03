using System.Text.Json;
using _1.Models;

namespace _1.Utils
{
    public class RepertoireService
    {
        private readonly string _filePath = Path.Combine("App_Data", "repertoire.json");

        public List<RepertoireEntry> GetAll()
        {
            if (!File.Exists(_filePath))
                return new List<RepertoireEntry>();

            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<RepertoireEntry>>(json) ?? new();
        }

        public void SaveAll(List<RepertoireEntry> entries)
        {
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public void Add(RepertoireEntry entry)
        {
            var entries = GetAll();
            entries.Add(entry);
            SaveAll(entries);
        }

        public void Update(Guid id, string htmlContent)
        {
            var entries = GetAll();
            var entry = entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                entry.HtmlContent = htmlContent;
                SaveAll(entries);
            }
        }

        public void Delete(Guid id)
        {
            var entries = GetAll();
            entries = entries.Where(e => e.Id != id).ToList();
            SaveAll(entries);
        }
    }
}
