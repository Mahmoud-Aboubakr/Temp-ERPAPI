using Api.CustomConfiguration;
using Application.Contracts.IServices;
using Application.Dtos.Inventory.Setup.Store;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Inventory.Setup;

public class StoresController : CommonBaseController
{
    public readonly IStoreService _storeService;
    public StoresController(IUnitOfWork uof, IMapper mapper,
        IHttpContextAccessor accessor, IResponseModelHandler responseModelHandler,
        IPaginatedModelHandler paginatedModelHandler, IOptions<CustomServiceConfiguration> options,
        IStoreService storeService) :
        base(uof, mapper, accessor, responseModelHandler, paginatedModelHandler, options)
    {
        _storeService = storeService;
    }

    [HttpGet("GetStores")]
    public async Task<IPaginatedModelHandler> GetStores([FromQuery] PaginationParams paginationParams)
    {
        using var cts = GetCommandCancellationToken();

        return await _storeService.GetStoresAsync(paginationParams.PageSize, paginationParams.PageNumber, paginationParams.Term, Lang, cts);
    }

    [HttpGet("GetStore/{id}")]
    public async Task<IResponseModel> GetStore([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();

        return await _storeService.GetStoreAsync(id, Lang, cts); 

    }

    [HttpPost("AddStore")]
    public async Task<IResponseModel> AddStore([FromBody] CreateStoreDto Store)
    {
        using var cts = GetCommandCancellationToken();
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(Store, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        return await _storeService.AddStoreAsync(Store, Lang, cts); 

    }

    [HttpPut("{id}")]
    public async Task<IResponseModel> UpdateStore(int id, [FromBody] UpdateStoreDto Store)
    {
        if (!ModelState.IsValid)
            return responseModelHandler.GetResponseModel(Store, "VALIDATION", StatusCodes.Status404NotFound, Lang);

        return await _storeService.UpdateStoreAsync(id, Store, Lang); 
    }

    [HttpDelete("{id}")]
    public async Task<IResponseModel> DeleteStore([FromRoute] int id)
    {
        using var cts = GetCommandCancellationToken();
        return await _storeService.DeleteStoreAsync(id, Lang, cts); 
    }
}
