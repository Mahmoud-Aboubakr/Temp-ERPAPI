using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.ItemCategory;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Inventory.Setup;

public class ItemCategoriesController : CommonBaseController
{
    public ItemCategoriesController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
    }

    [HttpGet("GetItemCategories")]
    public async Task<IPaginatedModelHandler> GetItemCategories([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();
        using (ISpecification<ItemCategory> specifications = new ItemCategorySpec(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term))
        {
            IEnumerable<ReadItemCategoryDto> itemCategorys = mapper.Map<IEnumerable<ReadItemCategoryDto>>(await ItemCategoryRepository.GetAllAsync(specifications));
            var itemCategorySearch = new BaseSpecification<ItemCategory>(a => a.Name.Contains(paginationParams.Term));
            IPaginatedModelHandler responseModel = paginatedModelHandler.Create(itemCategorys, paginationParams.PageNumber, paginationParams.PageSize, await ItemCategoryRepository.CountAsync(itemCategorySearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

            return responseModel;
        }


    }

    [HttpGet("GetItemCategory/{ItemCategoryId}")]
    public async Task<IResponseModel> GetItemCategory([FromRoute] int ItemCategoryId)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();
        using (ISpecification<ItemCategory> specifications = new ItemCategorySpec(ItemCategoryId))
        {
            ItemCategory ItemCategory = await ItemCategoryRepository.GetByIdAsync(ItemCategoryId, cts.Token);

            if (ItemCategory is null || ItemCategory.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

            return responseModelHandler.GetResponseModel(mapper.Map<ReadItemCategoryDto>(ItemCategory), "DONE", StatusCodes.Status200OK, Lang);
        }

    }

    [HttpPost("AddItemCategory")]
    public async Task<IResponseModel> AddItemCategory([FromBody] CreateItemCategoryDto ItemCategory)
    {
        using var cts = GetCommandCancellationToken();

        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemCategory, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();

        var itemCategorySpe = new BaseSpecification<ItemCategory>(a => a.Name == ItemCategory.Name);
        var checkItemCategoryName = await ItemCategoryRepository.GetAsync(itemCategorySpe);
        if (checkItemCategoryName != null)
            return responseModelHandler.GetResponseModel(ItemCategory, "ITEM_Category_NAME_EXIST", StatusCodes.Status409Conflict, Lang);

        ItemCategory newItemCategory = mapper.Map<ItemCategory>(ItemCategory);

        newItemCategory.IsActive = true;
        newItemCategory.CreateDate = DateTime.Now;

        await ItemCategoryRepository.InsertAsync(newItemCategory, cts.Token);
        await uof.Commit();
        return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);
    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdateItemCategory(int id, [FromBody] UpdateItemCategoryDto ItemCategory)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(ItemCategory, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        if (id != ItemCategory.Id)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();

        var itemCategorySpe = new BaseSpecification<ItemCategory>(a => a.Name == ItemCategory.Name);
        var checkItemCategoryName = await ItemCategoryRepository.GetAsync(itemCategorySpe);
        if (checkItemCategoryName != null && checkItemCategoryName.Id != id)
            return responseModelHandler.GetResponseModel(ItemCategory, "ITEM_Category_NAME_EXIST", StatusCodes.Status409Conflict, Lang);
        
        if (checkItemCategoryName != null)
            ItemCategoryRepository.Detach(checkItemCategoryName);
        ItemCategory updateItemCategory = mapper.Map<UpdateItemCategoryDto, ItemCategory>(ItemCategory);
        updateItemCategory.EditDate = DateTime.Now;

        ItemCategoryRepository.Update(updateItemCategory);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeleteItemCategory([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();

        ItemCategory currentItemCategory = await ItemCategoryRepository.GetByIdAsync(id, cts.Token);

        if (currentItemCategory is null || currentItemCategory.IsDeleted)
            return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

        currentItemCategory.DeleteDate = DateTime.Now;
        ItemCategoryRepository.SoftDelete(currentItemCategory);
        await uof.Commit();

        return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

    }
}
