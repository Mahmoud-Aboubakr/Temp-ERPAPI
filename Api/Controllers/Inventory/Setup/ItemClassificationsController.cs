using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.ItemClassification;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Inventory.Setup;

public class ItemClassificationsController : CommonBaseController
{
    public ItemClassificationsController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
    }

    [HttpGet("GetItemClassifications")]
    public async Task<IPaginatedModelHandler> GetItemClassifications([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();
        using (ISpecification<ItemClassification> specifications = new ItemClassificationSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
        {
            IEnumerable<ReadItemClassificationDto> itemClassifications = mapper.Map<IEnumerable<ReadItemClassificationDto>>(await ItemClassificationRepository.GetAllAsync(specifications));
            var itemClassificationSearch = new BaseSpecification<ItemClassification>(a => a.Name.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(itemClassifications, paginationParams.PageNumber, paginationParams.PageSize, await ItemClassificationRepository.CountAsync(itemClassificationSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            return responseModel;
        }


    }

    [HttpGet("GetItemClassification/{ItemClassificationId}")]
    public async Task<IResponseModel> GetItemClassification([FromRoute] int ItemClassificationId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();
        using (ISpecification<ItemClassification> specifications = new ItemClassificationSpec(ItemClassificationId))
        {
            ItemClassification ItemClassification = await ItemClassificationRepository.GetByIdAsync(ItemClassificationId, cts.Token);

            if (ItemClassification is null || ItemClassification.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            return responseModelHandler.GetResponseModel(mapper.Map<ReadItemClassificationDto>(ItemClassification), "DONE", StatusCodes.Status200OK, Lang);
        }

    }

    [HttpPost("AddItemClassification")]
    public async Task<IResponseModel> AddItemClassification([FromBody] CreateItemClassificationDto ItemClassification)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemClassification, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();

        var itemClassificationSpe = new BaseSpecification<ItemClassification>(a => a.Code == ItemClassification.Code);
        var checkItemClassificationName = await ItemClassificationRepository.GetAsync(itemClassificationSpe);
        if (checkItemClassificationName != null)
            return responseModelHandler.GetResponseModel(ItemClassification, "ITEM_Classification_CODE_EXIST", StatusCodes.Status409Conflict, Lang);

        ItemClassification newItemClassification = mapper.Map<ItemClassification>(ItemClassification);

        newItemClassification.IsActive = true;
        newItemClassification.CreateDate = DateTime.Now;

        await ItemClassificationRepository.InsertAsync(newItemClassification, cts.Token);
        await uof.Commit();
        return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdateItemClassification(int id, [FromBody] UpdateItemClassificationDto ItemClassification)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemClassification, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        if (id != ItemClassification.Id)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();
        var itemClassificationSpe = new BaseSpecification<ItemClassification>(a => a.Code == ItemClassification.Code);
        var checkItemClassificationName = await ItemClassificationRepository.GetAsync(itemClassificationSpe);
        if (checkItemClassificationName != null && checkItemClassificationName.Id != id)
            return responseModelHandler.GetResponseModel(ItemClassification, "ITEM_Classification_CODE_EXIST", StatusCodes.Status409Conflict, Lang);

        if (checkItemClassificationName != null)
            ItemClassificationRepository.Detach(checkItemClassificationName);

        ItemClassification updateItemClassification = mapper.Map<UpdateItemClassificationDto, ItemClassification>(ItemClassification);
        updateItemClassification.EditDate = DateTime.Now;

        ItemClassificationRepository.Update(updateItemClassification);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeleteItemClassification([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();

        ItemClassification currentItemClassification = await ItemClassificationRepository.GetByIdAsync(id, cts.Token);

        if (currentItemClassification is null || currentItemClassification.IsDeleted)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        currentItemClassification.DeleteDate = DateTime.Now;
        ItemClassificationRepository.SoftDelete(currentItemClassification);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

    }
}
