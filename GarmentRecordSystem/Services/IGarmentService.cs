using GarmentRecordSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Services
{
    public interface IGarmentService
    {
        void AddGarment(Garment garment);
        void UpdateGarment(uint garmentID);
        void DeleteGarment(uint garmentID);
        Garment SearchGarment(uint garmentID);
        List<Garment> SortGarments(string sortBy);
    }
}
