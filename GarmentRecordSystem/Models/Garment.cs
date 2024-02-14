using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Models
{
    public class Garment
    {
        private static uint nextId = 1;
        public uint GarmentID { get; init; }
        public string BrandName { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string Color { get; set; }
        public Size Size { get; set; }


        public Garment(string brandName, DateOnly purchaseDate, string color, Size size)
        {
            GarmentID = nextId++;
            BrandName = brandName;
            PurchaseDate = purchaseDate;
            Color = color;
            Size = size;
        }

        public override string ToString()
        {
            return "ID: " + GarmentID + ", brand name: " + BrandName + ", purchased: " + PurchaseDate + ", color: " + Color + ", size: " + Size;
        }

    }
}
