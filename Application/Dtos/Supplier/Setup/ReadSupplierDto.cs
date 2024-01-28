using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Sipplier.Setup
{
    public class ReadSupplierDto
    {
        public int Id { get; set; }
        [StringLength(45, ErrorMessage = "The {0} must be at most {1} characters long.")]
        public string SupplierName { get; set; }
    }
}
