using GarmentRecordSystem.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Services
{
    internal class JsonHandlerService : IJsonHandlerService
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
