using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarmentRecordSystem.Models
{
    public class Garment
    {
        public uint GarmentID { get; set; }
        public string BrandName { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public string Color { get; set; }
        public Size Size { get; set; }
    }
}
