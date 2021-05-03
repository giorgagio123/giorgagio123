using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PhoneStore.Services.Models.Products
{
    public class ProductFilterModel
    {
        public string Name { get; set; }
        
        public decimal? FromPrice { get; set; } 
        
        public decimal? ToPrice { get; set; } 

        public string Processor { get; set; }

        public int? Storage { get; set; }

        public string OperatingSystem { get; set; }
    }
}
