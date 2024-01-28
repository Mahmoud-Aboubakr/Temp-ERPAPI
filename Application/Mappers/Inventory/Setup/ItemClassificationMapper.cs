using Application.Dtos.Inventory.Setup.ItemClassification;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class ItemClassificationMapper : Profile
    {
        public ItemClassificationMapper()
        {
            CreateMap<ItemClassification, ReadItemClassificationDto>();
            CreateMap<CreateItemClassificationDto, ItemClassification>();
            CreateMap<UpdateItemClassificationDto, ItemClassification>();
        }
    }
}
