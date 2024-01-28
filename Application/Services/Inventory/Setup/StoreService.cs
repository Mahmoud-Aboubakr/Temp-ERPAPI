using Application.Contracts.Handlers;
using Application.Contracts.IServices;
using Application.Contracts.Models;
using Application.Contracts.Persistence;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Store;
using Application.Specifications;
using Application.Specifications.Inventory.Setup;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Inventory.Setup
{
    public class StoreService : IStoreService
    {
        private readonly IUnitOfWork uof;
        private readonly IMapper mapper;
        private readonly IResponseModelHandler responseModelHandler;
        private readonly IPaginatedModelHandler paginatedModelHandler;

        public UserManager<AppUser> UserManager { get; }

        public StoreService(IUnitOfWork unitOfWork, 
            IMapper _mapper,
            IResponseModelHandler _responseModelHandler, 
            IPaginatedModelHandler _paginatedModelHandler,
            UserManager<AppUser> userManager)
        {
            uof = unitOfWork;
            mapper = _mapper;
            responseModelHandler = _responseModelHandler;
            paginatedModelHandler = _paginatedModelHandler;
            UserManager = userManager;
        }
        public async Task<IPaginatedModelHandler> GetStoresAsync(int pageSize, int pageNumber, string term, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Store> storeRepository = uof.GetRepository<Store>();
            using (ISpecification<Store> specifications = new StoreSpec(pageSize, pageNumber, term))
            {
                specifications.IncludeStrings.AddRange(new List<string>() { "Department", "AppUser", "Branch" });
                IEnumerable<ReadStoreDto> Stores = mapper.Map<IEnumerable<ReadStoreDto>>(await storeRepository.GetAllAsync(specifications));
                var storeSearch = new BaseSpecification<Store>(a => a.StoreNameAr.Contains(term) || a.StoreNameEn.Contains(term));
                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(Stores, pageNumber, pageSize, await storeRepository.CountAsync(storeSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                return responseModel;
            }
        }
        public async Task<IResponseModel> GetStoreAsync(int id, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Store> storeRepository = uof.GetRepository<Store>();
            using (ISpecification<Store> specifications = new StoreSpec(id))
            {
                Store store = await storeRepository.GetByIdAsync(id, cts.Token);

                if (store is null || store.IsDeleted)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadStoreDto>(store), "DONE", StatusCodes.Status200OK, Lang);
            }
        }
        public async Task<IResponseModel> AddStoreAsync(CreateStoreDto Store, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Department> DepartmentRepository = uof.GetRepository<Department>();
            var findDepartment = await DepartmentRepository.GetByIdAsync(Store.DepartmentId);
            if (findDepartment == null)
                return responseModelHandler.GetResponseModel(Store, "DEPARTMENT_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //var findAppUser = await UserManager.FindByIdAsync(Store.AppUserId);
            //if (findAppUser == null)
            //    return responseModelHandler.GetResponseModel(Store, "STORE_ADMIN_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Branch> BranchRepository = uof.GetRepository<Branch>();
            var findBranchRepository = await BranchRepository.GetByIdAsync(Store.BranchId);
            if (findBranchRepository == null)
                return responseModelHandler.GetResponseModel(Store, "BRANCH_NOT_FOUND", StatusCodes.Status404NotFound, Lang);
            if(Store.Manual && (Store.StoreCode is null || Store.StoreCode.Trim() == "") )
                return responseModelHandler.GetResponseModel(Store, "STORE_CODE_REQUIRED", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Store> StoreRepository = uof.GetRepository<Store>();

            if (Store.StoreCode is not null && Store.StoreCode.Trim() != "")
            {
                //check if store code exist 
                var StoreSpe = new BaseSpecification<Store>(a => a.StoreCode == Store.StoreCode);
                var checkStoreCode = await StoreRepository.GetAsync(StoreSpe);
                if (checkStoreCode != null)
                    return responseModelHandler.GetResponseModel(Store, "STORE_CODE_EXSIT", StatusCodes.Status409Conflict, Lang);
            }

            if (Store.MainStoreId > 0)
            {
                var checkMainStore = await StoreRepository.GetByIdAsync(Store.MainStoreId);
                if (checkMainStore == null)
                    return responseModelHandler.GetResponseModel(Store, "MAIN_STORE_NOT_FOUND", StatusCodes.Status409Conflict, Lang);
                StoreRepository.Detach(checkMainStore);

                if (checkMainStore != null)
                    StoreRepository.Detach(checkMainStore);
            }

            Store newStore = mapper.Map<Store>(Store);

            newStore.IsActive = true;
            newStore.CreateDate = DateTime.Now;

            var inserted = await StoreRepository.InsertWithEntityAsync(newStore, cts.Token);
            if (inserted.StoreCode is null || inserted.StoreCode.Trim() == "")
            {
                inserted.StoreCode = inserted.Id.ToString();
                StoreRepository.Update(inserted);
                await uof.Commit();
            }
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }

        public async Task<IResponseModel> UpdateStoreAsync(int id, UpdateStoreDto Store, string Lang)
        {
            if (id != Store.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Department> DepartmentRepository = uof.GetRepository<Department>();
            var findDepartment = await DepartmentRepository.GetByIdAsync(Store.DepartmentId);
            if (findDepartment == null)
                return responseModelHandler.GetResponseModel(Store, "DEPARTMENT_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //var findAppUser = await UserManager.FindByIdAsync(Store.AppUserId);
            //if (findAppUser == null)
            //    return responseModelHandler.GetResponseModel(Store, "STORE_ADMIN_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Branch> BranchRepository = uof.GetRepository<Branch>();
            var findBranchRepository = await BranchRepository.GetByIdAsync(Store.BranchId);
            if (findBranchRepository == null)
                return responseModelHandler.GetResponseModel(Store, "BRANCH_NOT_FOUND", StatusCodes.Status404NotFound, Lang);
            if (Store.Manual && (Store.StoreCode is null || Store.StoreCode.Trim() == ""))
                return responseModelHandler.GetResponseModel(Store, "STORE_CODE_REQUIRED", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Store> StoreRepository = uof.GetRepository<Store>();

            if (Store.StoreCode is not null && Store.StoreCode.Trim() != "")
            {
                //check if store code exist 
                var StoreSpe = new BaseSpecification<Store>(a => a.StoreCode == Store.StoreCode);
                var checkStoreCode = await StoreRepository.GetAsync(StoreSpe);
                if (checkStoreCode != null && checkStoreCode.Id != id)
                    return responseModelHandler.GetResponseModel(Store, "STORE_CODE_EXSIT", StatusCodes.Status409Conflict, Lang);
                
                if (checkStoreCode != null)
                    StoreRepository.Detach(checkStoreCode);
            }

            if(Store.MainStoreId > 0)
            {
                var checkMainStore = await StoreRepository.GetByIdAsync(Store.MainStoreId); 
                if(checkMainStore == null)
                    return responseModelHandler.GetResponseModel(Store, "MAIN_STORE_NOT_FOUND", StatusCodes.Status409Conflict, Lang);
                StoreRepository.Detach(checkMainStore);

                if (checkMainStore != null)
                    StoreRepository.Detach(checkMainStore);
            }

            Store updateStore = mapper.Map<UpdateStoreDto, Store>(Store);
            updateStore.EditDate = DateTime.Now;

            if (updateStore.StoreCode is null || updateStore.StoreCode.Trim() == "")
                updateStore.StoreCode = updateStore.Id.ToString();

            StoreRepository.Update(updateStore);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
        
        public async Task<IResponseModel> DeleteStoreAsync(int id, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Store> StoreRepository = uof.GetRepository<Store>();

            Store currentStore = await StoreRepository.GetByIdAsync(id, cts.Token);

            if (currentStore is null || currentStore.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentStore.DeleteDate = DateTime.Now;
            StoreRepository.SoftDelete(currentStore);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
