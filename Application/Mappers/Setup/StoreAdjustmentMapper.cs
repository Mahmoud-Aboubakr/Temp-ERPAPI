using Application.Dtos.Setup.StoreAdjustment;
using AutoMapper;
using Domain.Entities.Setup;

namespace Application.Mappers.Setup
{
    public class StoreAdjustmentMapper : Profile
    {
        public StoreAdjustmentMapper()
        {
            CreateMap<StoreAdjustment, ReadStoreAdjustmentDto>();
            CreateMap<AddStoreAdjustmentDto, StoreAdjustment>();
            CreateMap<UpdateStoreAdjustmentDto, StoreAdjustment>();
        }
    }
}
