using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Contact;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Inventory.Setup;
using Domain.Entities.Cashier.Setup;
using Domain.Entities.Inventory;
using Domain.Entities.Inventory.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Inventory.Setup
{
    public class ContactTypesController : CommonBaseController
    {
        public ContactTypesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options) :
            base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
        {

        }


        [HttpGet("GetContacts")]
        public async Task<IPaginatedModelHandler> GetContacts([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ContactTypes> ContactRepository = uof.GetRepository<ContactTypes>();
            using (ISpecification<ContactTypes> specifications = new ContactsSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
            {
                IEnumerable<ReadContactDto> contacts = mapper.Map<IEnumerable<ReadContactDto>>(await ContactRepository.GetAllAsync(specifications, cts.Token));
                var ContactTypeSearch = new BaseSpecification<ContactTypes>(a => a.ArabicName.Contains(paginationParams.Term) || a.EnglishName.Contains(paginationParams.Term));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(contacts, paginationParams.PageNumber, paginationParams.PageSize, await ContactRepository.CountAsync(ContactTypeSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);
               
                return responseModel;
            }
        }
        [HttpGet("GetContact/{ContactId}")]
        public async Task<IResponseModel> GetContacts([FromRoute] int ContactId)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ContactTypes> ContactTypesRepository = uof.GetRepository<ContactTypes>();
            using (ISpecification<ContactTypes> specifications = new ContactsSpec(ContactId))
            {
                ContactTypes ContactTypes = await ContactTypesRepository.GetByIdAsync(ContactId, cts.Token);

                if (ContactTypes is null || ContactTypes.IsDeleted)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadContactDto>(ContactTypes), "DONE", StatusCodes.Status200OK, Lang);
            }

        }

        [HttpPost("AddContact")]
        public async Task<IResponseModel> AddContact([FromBody] CreateContactDto contact)
        {

            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(contact, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ContactTypes> ContactRepository = uof.GetRepository<ContactTypes>();
            ContactTypes newContact = mapper.Map<ContactTypes>(contact);
            newContact.IsActive = true;
            newContact.CreateDate = DateTime.Now;
            await ContactRepository.InsertAsync(newContact, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }

        [HttpPut("UpdateContact/{id}")]
        public async Task<IResponseModel> UpdateContact(int id, [FromBody] UpdateContactDto contact)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(contact, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != contact.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ContactTypes> ContactRepository = uof.GetRepository<ContactTypes>();
            ContactTypes updateContact = mapper.Map<UpdateContactDto, ContactTypes>(contact);

            ContactRepository.Update(updateContact);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }

        [HttpDelete("DeleteContact/{id}")]
        public async Task<IResponseModel> DeleteContact([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<ContactTypes> ContactRepository = uof.GetRepository<ContactTypes>();

            ContactTypes currentContact = await ContactRepository.GetByIdAsync(id, cts.Token);

            if (currentContact is null || currentContact.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentContact.DeleteDate = DateTime.Now;
            ContactRepository.SoftDelete(currentContact);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }

    }
}
