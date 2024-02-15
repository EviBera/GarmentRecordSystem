using GarmentRecordSystem.Models;
using Newtonsoft.Json;

namespace GarmentRecordSystem.Services
{
    public class JsonHandlerService : IJsonHandlerService
    {
        public List<Garment> LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Garment>>(json) ?? new List<Garment>();
        }

        public void SaveToFile(string filePath, List<Garment> garments)
        {
            var json = JsonConvert.SerializeObject(garments, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
