
using Application.Contracts.Models;
using Application.Dtos.Inventory.Setup.Item;
using Application.Dtos.Setup.IdentitificationTypes.Requests;
using Application.Dtos.Setup.IdentitificationTypes.Responses;
using Application.Models;
using Domain.Entities.HR.Setup;
using Domain.Entities.Inventory;
using Domain.Entities.Inventory.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.IServices
{
    public interface IItemService
    {
        Task<IPaginatedModelHandler> GetItemsAsync(int pageSize,  int pageNumber, string term, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> GetItemAsync(int ItemId, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> AddItemAsync(CreateItemDto createItemDto, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> UpdateItemAsync(int id, UpdateItemDto updateItemDto, string Lang);
        Task<IResponseModel> DeleteItemAsync(int id, string Lang, CancellationTokenSource cts);
    }
}
