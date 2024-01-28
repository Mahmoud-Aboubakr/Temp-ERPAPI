using Api.CustomConfiguration;
using Application.Contracts.Specifications;
using Application.Dtos.Cashier.Setup.PaymentGroup;
using Application.Specifications;
using Application.Specifications.Cashier.Setup;
using Domain.Entities.Cashier.Setup;
using Domain.Entities.Inventory.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Cashier.Setup;

public class PaymentGroupController : CommonBaseController
{

    public PaymentGroupController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
    }

    [HttpGet("GetPaymentGroups")]
    public async Task<IPaginatedModelHandler> GetPaymentGroups([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentGroup> PaymentGroupRepository = uof.GetRepository<PaymentGroup>();
        using (ISpecification<PaymentGroup> specifications = new PaymentGroupSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
        {
            IEnumerable<ReadPaymentGroupDto> PaymentGroups = mapper.Map<IEnumerable<ReadPaymentGroupDto>>(await PaymentGroupRepository.GetAllAsync(specifications));
            var PaymentGroupSearch = new BaseSpecification<PaymentGroup>(a => a.Name.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(PaymentGroups, paginationParams.PageNumber, paginationParams.PageSize, await PaymentGroupRepository.CountAsync(PaymentGroupSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            return responseModel;
        }


    }

    [HttpGet("GetPaymentGroup/{PaymentGroupId}")]
    public async Task<IResponseModel> GetPaymentGroup([FromRoute] int PaymentGroupId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentGroup> PaymentGroupRepository = uof.GetRepository<PaymentGroup>();
        using (ISpecification<PaymentGroup> specifications = new PaymentGroupSpec(PaymentGroupId))
        {
            PaymentGroup PaymentGroup = await PaymentGroupRepository.GetByIdAsync(PaymentGroupId, cts.Token);

            if (PaymentGroup is null || PaymentGroup.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            return responseModelHandler.GetResponseModel(mapper.Map<ReadPaymentGroupDto>(PaymentGroup), "DONE", StatusCodes.Status200OK, Lang);
        }

    }

    [HttpPost("AddPaymentGroup")]
    public async Task<IResponseModel> AddPaymentGroup([FromBody] CreatePaymentGroupDto PaymentGroup)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(PaymentGroup, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<PaymentGroup> PaymentGroupRepository = uof.GetRepository<PaymentGroup>();
        
        var paymentGroupSpec = new BaseSpecification<PaymentGroup>(a => a.Name == PaymentGroup.Name);
        var checkPaymentGroupName = await PaymentGroupRepository.GetAsync(paymentGroupSpec);
        if (checkPaymentGroupName != null)
            return responseModelHandler.GetResponseModel(PaymentGroup, "PAYMENT_GROUP_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        PaymentGroup newPaymentGroup = mapper.Map<PaymentGroup>(PaymentGroup);

        newPaymentGroup.IsActive = true;
        newPaymentGroup.CreateDate = DateTime.Now;

        await PaymentGroupRepository.InsertAsync(newPaymentGroup, cts.Token);
        await uof.Commit();
        return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdatePaymentGroup(int id, [FromBody] UpdatePaymentGroupDto PaymentGroup)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(PaymentGroup, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        if (id != PaymentGroup.Id)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<PaymentGroup> PaymentGroupRepository = uof.GetRepository<PaymentGroup>();

        var paymentGroupSpec = new BaseSpecification<PaymentGroup>(a => a.Name == PaymentGroup.Name);
        var checkPaymentGroupName = await PaymentGroupRepository.GetAsync(paymentGroupSpec);
        if (checkPaymentGroupName != null && checkPaymentGroupName.Id != id)
            return responseModelHandler.GetResponseModel(PaymentGroup, "PAYMENT_GROUP_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        if (checkPaymentGroupName != null)
            PaymentGroupRepository.Detach(checkPaymentGroupName);

        PaymentGroup updatePaymentGroup = mapper.Map<UpdatePaymentGroupDto, PaymentGroup>(PaymentGroup);
        updatePaymentGroup.EditDate = DateTime.Now;

        PaymentGroupRepository.Update(updatePaymentGroup);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeletePaymentGroup([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<PaymentGroup> PaymentGroupRepository = uof.GetRepository<PaymentGroup>();

        PaymentGroup currentPaymentGroup = await PaymentGroupRepository.GetByIdAsync(id, cts.Token);

        if (currentPaymentGroup is null || currentPaymentGroup.IsDeleted)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        currentPaymentGroup.DeleteDate = DateTime.Now;
        PaymentGroupRepository.SoftDelete(currentPaymentGroup);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

    }
}
