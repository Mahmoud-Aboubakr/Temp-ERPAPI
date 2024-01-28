using Application.Dtos.Inventory.Setup.ItemCategory;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class ItemCategoryMapper : Profile
    {
        public ItemCategoryMapper()
        {
            CreateMap<ItemCategory, ReadItemCategoryDto>();
            CreateMap<CreateItemCategoryDto, ItemCategory>();
            CreateMap<UpdateItemCategoryDto, ItemCategory>();
        }
    }
}
