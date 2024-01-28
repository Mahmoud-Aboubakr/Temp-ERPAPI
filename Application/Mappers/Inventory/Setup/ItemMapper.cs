using Application.Dtos.Inventory.Setup.Item;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ReadItemDto>();
            CreateMap<CreateItemDto, Item>();
            CreateMap<UpdateItemDto, Item>();
        }
    }
}
