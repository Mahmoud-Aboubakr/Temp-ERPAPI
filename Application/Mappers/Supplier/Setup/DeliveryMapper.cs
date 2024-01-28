using Application.Dtos.Sipplier.Setup;
using Application.Dtos.Supplier.Delivery;
using Application.Dtos.Supplier.Setup;
using AutoMapper;
using Domain.Entities.Supplier.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers.Supplier.Setup
{
    public class DeliveryMapper : Profile
    {
        public DeliveryMapper()
        {
            CreateMap<CreationDeliveryDto, Delivery>();
            CreateMap<UpdateDeliveryDto, Delivery>();
            CreateMap<Delivery, ReadDeliveryDto>();
        }
    }   
}
