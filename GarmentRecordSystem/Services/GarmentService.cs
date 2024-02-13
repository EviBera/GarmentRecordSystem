using GarmentRecordSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Services
{
    public class GarmentService : IGarmentService
    {

        private List<Garment> _garments = new List<Garment>();

        public void AddGarment(Garment garment)
        {
            _garments.Add(garment);
        }

        public void DeleteGarment(uint garmentID)
        {
            _garments.RemoveAll(g => g.GarmentID == garmentID);
        }

        public Garment SearchGarment(uint garmentID)
        {
            return _garments.FirstOrDefault(g => g.GarmentID == garmentID);
        }

        public List<Garment> SortGarments(SortGarmentsCriteria sortBy)
        {
            switch (sortBy)
            {
                case SortGarmentsCriteria.GarmentID:
                    return _garments.OrderBy(g => g.GarmentID).ToList();
                case SortGarmentsCriteria.BrandName:
                    return _garments.OrderBy(g => g.BrandName).ToList();
                case SortGarmentsCriteria.PurchaseDate:
                    return _garments.OrderBy(g => g.PurchaseDate).ToList();
                case SortGarmentsCriteria.Color:
                    return _garments.OrderBy(g => g.Color).ToList();
                case SortGarmentsCriteria.Size:
                    return _garments.OrderBy(g => g.Size).ToList();
                default:
                    throw new ArgumentException("Invalid sorting criteria");
            }
        }

        public void UpdateGarment(uint garmentID, Garment newGarment)
        {
            var oldGarment = _garments.FirstOrDefault(g => g.GarmentID == garmentID);
            if (oldGarment != null)
            {
                oldGarment = newGarment;
            } 
            else
            {
                throw new Exception("Garment not found");
            }
        }
    }
}
