using Application.Dtos.Inventory.Setup.Itemtype;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class ItemTypeMapper : Profile
    {
        public ItemTypeMapper()
        {
            CreateMap<ItemType, ReadItemTypeDto>();
            CreateMap<CreateItemTypeDto, ItemType>();
            CreateMap<UpdateItemTypeDto, ItemType>();
        }
    }
}
