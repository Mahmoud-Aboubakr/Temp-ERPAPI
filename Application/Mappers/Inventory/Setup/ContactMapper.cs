using Application.Dtos.Inventory.Setup.Contact;
using AutoMapper;
using Domain.Entities.Inventory.Setup;

namespace Application.Mappers.Inventory.Setup
{
    public class ContactMapper : Profile
    {
        public ContactMapper()
        {
            CreateMap<CreateContactDto, ContactTypes>();
            CreateMap<UpdateContactDto, ContactTypes>();
            CreateMap<ContactTypes, ReadContactDto>();
        }
        
    }
}
