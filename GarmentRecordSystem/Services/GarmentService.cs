﻿using GarmentRecordSystem.Models;

namespace GarmentRecordSystem.Services
{
    public class GarmentService : IGarmentService
    {

        private List<Garment> _garments;
        private readonly IJsonHandlerService _jsonHandlerService;

        public GarmentService(IJsonHandlerService jsonHandlerService)
        {
            _jsonHandlerService = jsonHandlerService;
            _garments = new List<Garment>();
        }

        public void AddGarment(Garment garment)
        {
            _garments.Add(garment);
        }

        public void DeleteGarment(uint garmentID)
        {
            _garments.RemoveAll(g => g.GarmentID == garmentID);
        }

        public Garment? SearchGarment(uint garmentID)
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

        public void UpdateGarment(uint garmentID, string brandName, DateOnly purchase, string color, Size size)
        {
            var oldGarment = _garments.FirstOrDefault(g => g.GarmentID == garmentID);
            if (oldGarment != null)
            {
                oldGarment.BrandName = brandName;
                oldGarment.PurchaseDate = purchase;
                oldGarment.Color = color;
                oldGarment.Size = size;
            } 
            else
            {
                throw new Exception("Garment not found");
            }
        }

        //Provide a safe copy
        public List<Garment> GetAllGarments()
        {
            return new List<Garment>(_garments);
        }

        //Provide access to the user-chosen file
        public void LoadGarments(string filePath)
        {
            _garments = _jsonHandlerService.LoadFromFile(filePath);
        }
    }
}
