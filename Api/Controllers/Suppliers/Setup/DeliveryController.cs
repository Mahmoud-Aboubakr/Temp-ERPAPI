using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.Sipplier.Setup;
using Application.Dtos.Supplier.Delivery;
using Application.Dtos.Supplier.Setup;
using Application.Specifications;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Supplier.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Supplier.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Suppliers.Setup
{
    public class DeliveryController : CommonBaseController
    {
        public DeliveryController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options) : 
            base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
        {
        }

        #region Get
        [HttpGet("GetDelivery")]
        public async Task<IPaginatedModelHandler> GetDeliveries([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Delivery> DeliveryRepository = uof.GetRepository<Delivery>();
            using (ISpecification<Delivery> specifications = new DeliveriesSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term ))
            {
                IEnumerable<ReadDeliveryDto> Deliveries = mapper.Map<IEnumerable<ReadDeliveryDto>>(await DeliveryRepository.GetAllAsync(specifications, cts.Token));
                var DeliveryeSearch = new BaseSpecification<Delivery>(a => a.DeliveryTerm.Contains(paginationParams.Term) );

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(Deliveries, paginationParams.PageNumber, paginationParams.PageSize, await DeliveryRepository.CountAsync(DeliveryeSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }
        #endregion

        #region getbyId
        [HttpGet("GetDelivery/{DeliveryId}")]
        public async Task<IResponseModel> GetDelivery([FromRoute] int DeliveryId)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Delivery> DeliveryRepository = uof.GetRepository<Delivery>();
            using (ISpecification<Delivery> specifications = new DeliveriesSpec(DeliveryId))
            {
                Delivery Delivery = await DeliveryRepository.GetByIdAsync(DeliveryId, cts.Token);

                if (Delivery is null || Delivery.IsDeleted)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadDeliveryDto>(Delivery), "DONE", StatusCodes.Status200OK, Lang);
            }

        }
        #endregion
        #region Add
        [HttpPost("AddDelivery")]
        public async Task<IResponseModel> AddDelivery([FromBody] CreationDeliveryDto delivery)
        {

            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(delivery, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Delivery> DeliveryRepository = uof.GetRepository<Delivery>();
            Delivery newDelivery = mapper.Map<Delivery>(delivery);
            newDelivery.IsActive = true;
            newDelivery.CreateDate = DateTime.Now;
            await DeliveryRepository.InsertAsync(newDelivery, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }
        #endregion

        #region Update

        [HttpPut("UpdateDelivery/{id}")]
        public async Task<IResponseModel> UpdateDelivery(int id, [FromBody] UpdateDeliveryDto delivery)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(delivery, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != delivery.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Delivery> DeliveryRepository = uof.GetRepository<Delivery>();
            Delivery updateDelivery = mapper.Map<UpdateDeliveryDto, Delivery>(delivery);

            DeliveryRepository.Update(updateDelivery);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion

        #region Delete
        [HttpDelete("DeleteDelivery/{id}")]
        public async Task<IResponseModel> DeleteDelivery([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<Delivery> DeliveryRepository = uof.GetRepository<Delivery>();

            Delivery currentDelivery = await DeliveryRepository.GetByIdAsync(id, cts.Token);

            if (currentDelivery is null || currentDelivery.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentDelivery.DeleteDate = DateTime.Now;
            DeliveryRepository.SoftDelete(currentDelivery);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion

    }
}
