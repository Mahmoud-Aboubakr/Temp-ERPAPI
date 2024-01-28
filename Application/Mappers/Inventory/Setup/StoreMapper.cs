using Application.Dtos.Inventory.Setup.Store;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class StoreMapper : Profile
    {
        public StoreMapper()
        {
            CreateMap<Store, ReadStoreDto>();
            CreateMap<CreateStoreDto, Store>();
            CreateMap<UpdateStoreDto, Store>();
        }
    }
}
