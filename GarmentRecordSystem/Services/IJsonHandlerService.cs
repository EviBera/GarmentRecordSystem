using GarmentRecordSystem.Models;

namespace GarmentRecordSystem.Services
{
    public interface IJsonHandlerService
    {
        void SaveToFile(string filePath, List<Garment> garments);
        List<Garment> LoadFromFile(string filePath);
    }
}
