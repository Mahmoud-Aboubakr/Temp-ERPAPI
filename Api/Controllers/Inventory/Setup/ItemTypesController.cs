using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Itemtype;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace Api.Controllers.Inventory.Setup;

public class ItemTypesController : CommonBaseController
{
    public ItemTypesController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
    }

    [HttpGet("GetItemTypes")]
    public async Task<IPaginatedModelHandler> GetItemTypes([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
        using (ISpecification<ItemType> specifications = new ItemTypeSpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
        {
            IEnumerable<ReadItemTypeDto> itemtypes = mapper.Map<IEnumerable<ReadItemTypeDto>>(await ItemTypeRepository.GetAllAsync(specifications));
            var itemTypeSearch = new BaseSpecification<ItemType>(a => a.TypeName.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(itemtypes, paginationParams.PageNumber, paginationParams.PageSize, await ItemTypeRepository.CountAsync(itemTypeSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            return responseModel;
        }


    }

    [HttpGet("GetItemType/{ItemTypeId}")]
    public async Task<IResponseModel> GetItemType([FromRoute] int ItemTypeId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
        using (ISpecification<ItemType> specifications = new ItemTypeSpec(ItemTypeId))
        {
            ItemType ItemType = await ItemTypeRepository.GetByIdAsync(ItemTypeId, cts.Token);

            if (ItemType is null || ItemType.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            return responseModelHandler.GetResponseModel(mapper.Map<ReadItemTypeDto>(ItemType), "DONE", StatusCodes.Status200OK, Lang);
        }

    }

    [HttpPost("AddItemType")]
    public async Task<IResponseModel> AddItemType([FromBody] CreateItemTypeDto ItemType)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemType, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
        var itemTypeSpe = new BaseSpecification<ItemType>(a => a.TypeName == ItemType.TypeName);
        var checkItemTypeName = await ItemTypeRepository.GetAsync(itemTypeSpe);
        if (checkItemTypeName != null)
            return responseModelHandler.GetResponseModel(ItemType, "ITEM_TYPE_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        ItemType newItemType = mapper.Map<ItemType>(ItemType);

        newItemType.IsActive = true;
        newItemType.CreateDate = DateTime.Now;

        await ItemTypeRepository.InsertAsync(newItemType, cts.Token);
        await uof.Commit();
        return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdateItemType(int id, [FromBody] UpdateItemTypeDto ItemType)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemType, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        if (id != ItemType.Id)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
        var itemTypeSpe = new BaseSpecification<ItemType>(a => a.TypeName == ItemType.TypeName);
        var checkItemTypeName = await ItemTypeRepository.GetAsync(itemTypeSpe);
        if (checkItemTypeName != null && checkItemTypeName.Id != id)
            return responseModelHandler.GetResponseModel(ItemType, "ITEM_TYPE_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        if (checkItemTypeName != null)
            ItemTypeRepository.Detach(checkItemTypeName);

        ItemType updateItemType = mapper.Map<UpdateItemTypeDto, ItemType>(ItemType);
        updateItemType.EditDate = DateTime.Now;

        ItemTypeRepository.Update(updateItemType);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeleteItemType([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();

        ItemType currentItemType = await ItemTypeRepository.GetByIdAsync(id, cts.Token);

        if (currentItemType is null || currentItemType.IsDeleted)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        currentItemType.DeleteDate = DateTime.Now;
        ItemTypeRepository.SoftDelete(currentItemType);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

    }
}
