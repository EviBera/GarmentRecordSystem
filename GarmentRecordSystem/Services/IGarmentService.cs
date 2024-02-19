using GarmentRecordSystem.Models;

namespace GarmentRecordSystem.Services
{
    public interface IGarmentService
    {
        void AddGarment(Garment garment);
        void UpdateGarment(uint garmentID, string brandName, DateOnly purchase, string color, Size size);
        void DeleteGarment(uint garmentID);
        Garment? SearchGarment(uint garmentID);
        List<Garment> SortGarments(SortGarmentsCriteria sortBy);
        List<Garment> GetAllGarments();
        void LoadGarments(string filePath);
    }
}
