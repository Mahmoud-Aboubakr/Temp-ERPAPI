using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Inventory.Setup.Contact
{
    public class UpdateContactDto
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
    }
}
