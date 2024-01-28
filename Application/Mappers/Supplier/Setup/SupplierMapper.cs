using Application.Dtos.Inventory.Setup.Contact;
using Application.Dtos.Sipplier.Setup;
using Application.Dtos.Supplier.Setup;
using AutoMapper;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.Supplier.Setup
{
    public class SupplierMapper: Profile
    {
        public SupplierMapper()
        {
            CreateMap<CreationSupplierDto,SupplierType>();
            CreateMap<UpdateSupplierDto,SupplierType>();
            CreateMap<SupplierType, ReadSupplierDto>();
        }
    }
}
