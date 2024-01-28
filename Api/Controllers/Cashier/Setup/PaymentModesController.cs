using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.Cashier.Setup.PaymentModes;
using Application.Specifications;
using Application.Specifications.Cashier.Setup;
using Domain.Entities.Cashier.Setup;
using Domain.Entities.Inventory.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Cashier.Setup;

public class PaymentModesController : CommonBaseController
{

    public PaymentModesController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
    }

    [HttpGet("GetPaymentModes")]
    public async Task<IPaginatedModelHandler> GetPaymentModes([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentModes> PaymentModesRepository = uof.GetRepository<PaymentModes>();
        using (ISpecification<PaymentModes> specifications = new PaymentModesSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
        {
            specifications.IncludeStrings.Add("PaymentModesType");
            IEnumerable<ReadPaymentModesDto> PaymentModess = mapper.Map<IEnumerable<ReadPaymentModesDto>>(await PaymentModesRepository.GetAllAsync(specifications));
            var PaymentModesSearch = new BaseSpecification<PaymentModes>(a => a.PaymentModeName.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(PaymentModess, paginationParams.PageNumber, paginationParams.PageSize, await PaymentModesRepository.CountAsync(PaymentModesSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            return responseModel;
        }


    }

    [HttpGet("GetPaymentModes/{PaymentModesId}")]
    public async Task<IResponseModel> GetPaymentModes([FromRoute] int PaymentModesId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentModes> PaymentModesRepository = uof.GetRepository<PaymentModes>();
        using (ISpecification<PaymentModes> specifications = new PaymentModesSpec(PaymentModesId))
        {
            PaymentModes PaymentModes = await PaymentModesRepository.GetByIdAsync(PaymentModesId, cts.Token);

            if (PaymentModes is null || PaymentModes.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            return responseModelHandler.GetResponseModel(mapper.Map<ReadPaymentModesDto>(PaymentModes), "DONE", StatusCodes.Status200OK, Lang);
        }

    }

    [HttpPost("AddPaymentModes")]
    public async Task<IResponseModel> AddPaymentModes([FromBody] CreatePaymentModesDto PaymentModes)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(PaymentModes, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<PaymentModesType> PaymentModesTypeRepository = uof.GetRepository<PaymentModesType>();
        var findPaymentModesType = await PaymentModesTypeRepository.GetByIdAsync(PaymentModes.PaymentModesTypeId);
        if (findPaymentModesType == null)
            return responseModelHandler.GetResponseModel(PaymentModes, "PAYMENT_MODE_TYPE_NOT_FOUND", StatusCodes.Status404NotFound, Lang);


        IGenericRepository<PaymentModes> PaymentModesRepository = uof.GetRepository<PaymentModes>();
        
        var PaymentModesSpec = new BaseSpecification<PaymentModes>(a => a.PaymentModeName == PaymentModes.PaymentModeName);
        var checkPaymentModesName = await PaymentModesRepository.GetAsync(PaymentModesSpec);
        if (checkPaymentModesName != null)
            return responseModelHandler.GetResponseModel(PaymentModes, "PAYMENT_MODE_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        PaymentModes newPaymentModes = mapper.Map<PaymentModes>(PaymentModes);

        newPaymentModes.CreateDate = DateTime.Now;

        await PaymentModesRepository.InsertAsync(newPaymentModes, cts.Token);
        await uof.Commit();
        return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdatePaymentModes(int id, [FromBody] UpdatePaymentModesDto PaymentModes)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(PaymentModes, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        if (id != PaymentModes.Id)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<PaymentModesType> PaymentModesTypeRepository = uof.GetRepository<PaymentModesType>();
        var findPaymentModesType = await PaymentModesTypeRepository.GetByIdAsync(PaymentModes.PaymentModesTypeId);
        if (findPaymentModesType == null)
            return responseModelHandler.GetResponseModel(PaymentModes, "PAYMENT_MODE_TYPE_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<PaymentModes> PaymentModesRepository = uof.GetRepository<PaymentModes>();

        var PaymentModesSpec = new BaseSpecification<PaymentModes>(a => a.PaymentModeName == PaymentModes.PaymentModeName);
        var checkPaymentModesName = await PaymentModesRepository.GetAsync(PaymentModesSpec);
        if (checkPaymentModesName != null && checkPaymentModesName.Id != id)
            return responseModelHandler.GetResponseModel(PaymentModes, "PAYMENT_MODE_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        if (checkPaymentModesName != null)
            PaymentModesRepository.Detach(checkPaymentModesName);

        PaymentModes updatePaymentModes = mapper.Map<UpdatePaymentModesDto, PaymentModes>(PaymentModes);
        updatePaymentModes.EditDate = DateTime.Now;

        PaymentModesRepository.Update(updatePaymentModes);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeletePaymentModes([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentModes> PaymentModesRepository = uof.GetRepository<PaymentModes>();

        PaymentModes currentPaymentModes = await PaymentModesRepository.GetByIdAsync(id, cts.Token);

        if (currentPaymentModes is null || currentPaymentModes.IsDeleted)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        currentPaymentModes.DeleteDate = DateTime.Now;
        PaymentModesRepository.SoftDelete(currentPaymentModes);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

    }
}
