
using Application.Contracts.Models;
using Application.Dtos.Inventory.Setup.Store;
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
    public interface IStoreService
    {
        Task<IPaginatedModelHandler> GetStoresAsync(int pageSize,  int pageNumber, string term, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> GetStoreAsync(int StoreId, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> AddStoreAsync(CreateStoreDto createStoreDto, string Lang, CancellationTokenSource cts);
        Task<IResponseModel> UpdateStoreAsync(int id, UpdateStoreDto updateStoreDto, string Lang);
        Task<IResponseModel> DeleteStoreAsync(int id, string Lang, CancellationTokenSource cts);
    }
}
