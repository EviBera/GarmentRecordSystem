using GarmentRecordSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Services
{
    public interface IJsonHandlerService
    {
        void SaveToFile(string filePath, List<Garment> garments);
        List<Garment> LoadFromFile(string filePath);
    }
}
