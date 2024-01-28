using Api.Controllers;
using Api.CustomConfiguration;
using Application;
using Application.Contracts.IServices;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Item;
using Application.Specifications;
using Application.Specifications.Inventory;
using Application.Specifications.Inventory.Setup;
using Application.Specifications.Setup;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Inventory.Setup;

public class ItemsController : CommonBaseController
{
    public IItemService ItemService { get; }

    public ItemsController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options,
        IItemService itemService) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
        ItemService = itemService;
    }

    [HttpGet("GetItems")]
    public async Task<IPaginatedModelHandler> GetItems([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        return await ItemService.GetItemsAsync(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term, Lang, cts);
    }

    [HttpGet("GetItem/{ItemId}")]
    public async Task<IResponseModel> GetItem([FromRoute] int ItemId)
    {
        using var cts = GetCommandCancellationToken();

        return await ItemService.GetItemAsync(ItemId, Lang, cts); 

    }

    [HttpPost("AddItem")]
    public async Task<IResponseModel> AddItem([FromBody] CreateItemDto Item)
    {
        using var cts = GetCommandCancellationToken();
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(Item, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        return await ItemService.AddItemAsync(Item, Lang, cts); 

    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdateItem(int id, [FromBody] UpdateItemDto Item)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(Item, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        return await ItemService.UpdateItemAsync(id, Item, Lang); 
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeleteItem([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();
        return await ItemService.DeleteItemAsync(id, Lang, cts); 
    }
}
