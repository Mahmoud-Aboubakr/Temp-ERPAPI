using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Contact;
using Application.Dtos.Sipplier.Setup;
using Application.Dtos.Supplier.Delivery;
using Application.Dtos.Supplier.Setup;
using Application.Specifications;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Supplier.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Supplier.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Suppliers.Setup
{
    public class SupplierTypesController : CommonBaseController
    {
        public SupplierTypesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options)
            : base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
        {
        }

        #region Get
        [HttpGet("GetSupplier")]
        public async Task<IPaginatedModelHandler> GetSuppliers([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<SupplierType> SupplierRepository = uof.GetRepository<SupplierType>();
            using (ISpecification<SupplierType> specifications = new SuppliersSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
            {
                IEnumerable<ReadSupplierDto> suppliers = mapper.Map<IEnumerable<ReadSupplierDto>>(await SupplierRepository.GetAllAsync(specifications, cts.Token));

                var SupplierTypeSearch = new BaseSpecification<SupplierType>(a => a.SupplierName.Contains(paginationParams.Term));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(suppliers, paginationParams.PageNumber, paginationParams.PageSize, await SupplierRepository.CountAsync(SupplierTypeSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }
        #endregion

        #region getById 
        [HttpGet("GetSupplierType/{SupplierId}")]
        public async Task<IResponseModel> GetSupplierType([FromRoute] int SupplierId)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<SupplierType> SupplierTypeRepository = uof.GetRepository<SupplierType>();
            using (ISpecification<SupplierType> specifications = new SuppliersSpec(SupplierId))
            {
                SupplierType SupplierType = await SupplierTypeRepository.GetByIdAsync(SupplierId, cts.Token);

                if (SupplierType is null || SupplierType.IsDeleted)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadSupplierDto>(SupplierType), "DONE", StatusCodes.Status200OK, Lang);
            }

        }
        #endregion
        #region Add
        [HttpPost("AddSupplier")]
        public async Task<IResponseModel> AddSupplier([FromBody] CreationSupplierDto supplier)
        {

            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(supplier, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<SupplierType> SupplierRepository = uof.GetRepository<SupplierType>();
            SupplierType newSupplier = mapper.Map<SupplierType>(supplier);
            newSupplier.IsActive = true;
            newSupplier.CreateDate = DateTime.Now;
            await SupplierRepository.InsertAsync(newSupplier, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }
        #endregion

        #region Update

        [HttpPut("UpdateSupplier/{id}")]
        public async Task<IResponseModel> UpdateSupplier(int id, [FromBody] UpdateSupplierDto supplier)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(supplier, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != supplier.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<SupplierType> SupplierRepository = uof.GetRepository<SupplierType>();
            SupplierType updateSupplier = mapper.Map<UpdateSupplierDto, SupplierType>(supplier);
            updateSupplier.EditDate = DateTime.Now;
            SupplierRepository.Update(updateSupplier);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion
         
        #region Delete
        [HttpDelete("DeleteSupplier/{id}")]
        public async Task<IResponseModel> DeleteSupplier([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<SupplierType> SupplierRepository = uof.GetRepository<SupplierType>();

            SupplierType currentSupplier = await SupplierRepository.GetByIdAsync(id, cts.Token);

            if (currentSupplier is null || currentSupplier.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentSupplier.DeleteDate = DateTime.Now;
            SupplierRepository.SoftDelete(currentSupplier);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion
    }
}
