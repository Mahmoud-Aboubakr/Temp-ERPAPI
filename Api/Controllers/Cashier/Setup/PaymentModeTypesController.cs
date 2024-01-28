using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.Cashier.Setup.PaymentModesType;
using Application.Specifications.Cashier.Setup;
using Domain.Entities.Cashier.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Cashier.Setup
{
    public class PaymentModeTypesController : CommonBaseController
    {
        public PaymentModeTypesController(IUnitOfWork uof, IMapper mapper, IHttpContextAccessor accessor,
            IResponseModelHandler responseModelHandler, IPaginatedModelHandler paginatedModelHandler,
            IOptions<CustomServiceConfiguration> options) : 
            base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
        {
        }

        #region Get
        [HttpGet("GetPaymentModesTypes")]
        public async Task<IPaginatedModelHandler> GetPaymentModesTypes([FromQuery] PaginationParams paginationParams)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<PaymentModesType> PaymentModesTypesRepository = uof.GetRepository<PaymentModesType>();
            using (ISpecification<PaymentModesType> specifications = new PaymentModesTypesSpec(paginationParams.PageSize, paginationParams.PageNumber))
            {
                IEnumerable<ReadPaymentModesTypeDto> payments = mapper.Map<IEnumerable<ReadPaymentModesTypeDto>>(await PaymentModesTypesRepository.GetAllAsync(specifications, cts.Token));

                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(payments, paginationParams.PageNumber, paginationParams.PageSize, await PaymentModesTypesRepository.CountAsync(cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                AddPaginationHeader(responseModel.CurrentPage, responseModel.PageSize, responseModel.TotalCount);

                return responseModel;
            }
        }
        #endregion
        #region Add
        [HttpPost("AddPaymentModesType")]
        public async Task<IResponseModel> AddPaymentModesType([FromBody] CreationPaymentModesTypeDto payment)
        {

            using var cts = GetCommandCancellationToken();

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(payment, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<PaymentModesType> PaymentRepository = uof.GetRepository<PaymentModesType>();
            PaymentModesType newPayment = mapper.Map<PaymentModesType>(payment);
            newPayment.IsActive = true;
            newPayment.CreateDate = DateTime.Now;
            await PaymentRepository.InsertAsync(newPayment, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }
        #endregion
        #region Update

        [HttpPut("UpdatePaymentModesType/{id}")]
        public async Task<IResponseModel> UpdatePaymentModesType(int id, [FromBody] UpdatePaymentModesTypeDto payment)
        {

            if (!ModelState.IsValid)
                return responseModelHandler.GetResponseModel(payment, "VALIDATION", StatusCodes.Status404NotFound, Lang);

            if (id != payment.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<PaymentModesType> PaymentRepository = uof.GetRepository<PaymentModesType>();

            PaymentModesType updatePayment = mapper.Map<UpdatePaymentModesTypeDto, PaymentModesType>(payment);
            updatePayment.EditDate = DateTime.Now;
            PaymentRepository.Update(updatePayment);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion
        #region Delete
        [HttpDelete("DeletePayment/{id}")]
        public async Task<IResponseModel> DeletePayment([FromRoute] int id)
        {
            using var cts = GetCommandCancellationToken();

            IGenericRepository<PaymentModesType> PaymentRepository = uof.GetRepository<PaymentModesType>();

            PaymentModesType currentPayment = await PaymentRepository.GetByIdAsync(id, cts.Token);

            if (currentPayment is null || currentPayment.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentPayment.DeleteDate = DateTime.Now;
            PaymentRepository.SoftDelete(currentPayment);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        #endregion
    }
}
